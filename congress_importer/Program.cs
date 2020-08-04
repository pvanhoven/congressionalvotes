using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Dapper;

namespace congress_importer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            if (args == null || args.Length < 4)
            {
                Console.WriteLine("Need 4 parameters: database, user, pass, path");
                return;
            }

            string databaseName = args[0];
            string userName = args[1];
            string password = args[2];
            //string path = args[3];
            string path = Directory.GetCurrentDirectory();
            path += "/../congress_downloader";

            SenateXmlLoader loader = new SenateXmlLoader();
            var sessions = loader.LoadSessions(path).OrderByDescending(s => s.Year).ThenBy(s => s.Session);

            foreach (SenateSession session in sessions)
            {
                using(SqlConnection connection = new SqlConnection($"Server=localhost;Database={databaseName};User Id={userName};Password={password};"))
                {
                    connection.Open();
                    int currentSessionId = 0;
                    using(SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $@"
                            BEGIN TRAN t1;
                            DECLARE @IdValue INT;

                            SELECT @IdValue = Id FROM dbo.Sessions WHERE CongressNumber = @CongressNumber AND SessionNumber = @SessionNumber;
                            IF @IdValue IS NULL
                            BEGIN
                                INSERT INTO dbo.Sessions VALUES (@CongressNumber, @SessionNumber, @Year)
                                SELECT @IdValue = @@IDENTITY;
                            END

                            SELECT @IdValue;
                            COMMIT TRAN t1;
                        ";

                        command.Parameters.Add("@CongressNumber", SqlDbType.Int).Value = session.Congress;
                        command.Parameters.Add("@SessionNumber", SqlDbType.Int).Value = session.Session;
                        command.Parameters.Add("@Year", SqlDbType.Int).Value = session.Year;

                        Console.WriteLine($"Insert/update session -> congress: '{session.Congress}' session: '{session.Session}'");
                        object result = command.ExecuteScalar();
                        currentSessionId = (int) result;
                    }

                    IEnumerable<RollCallVote> votes = loader.GetRollCallVotes(path, session.Congress, session.Session)
                        .OrderBy(v => Int32.TryParse(v.VoteNumber, out int voteNum) ? voteNum : -1);

                    var groupedMembers = votes
                        .SelectMany(v => v.Members.MemberElements)
                        .GroupBy(m => m.LisMemberId)
                        .Select(g => g.First())
                        .Select(m => new KeyValuePair<string, Member>(m.LisMemberId, m));

                    foreach (var kvpMember in groupedMembers)
                    {
                        var member = kvpMember.Value;
                        using SqlCommand command = connection.CreateCommand();
                        command.CommandText = $@"
                                BEGIN TRAN t1;
                                UPDATE dbo.Senators SET FullName = @FullName, LastName = @LastName, @FirstName = @FirstName, Party = @Party, State = @State
                                    WHERE LisMemberId = @LisMemberId
                                
                                IF @@RowCount = 0
                                BEGIN
                                    INSERT INTO dbo.Senators (FullName, LastName, FirstName, Party, State, LisMemberId) VALUES (@FullName, @LastName, @FirstName, @Party, @State, @LisMemberId)
                                END
                                COMMIT TRAN t1;
                            ";

                        command.Parameters.AddWithValue("@FullName", member.FullName);
                        command.Parameters.AddWithValue("@LastName", member.LastName);
                        command.Parameters.AddWithValue("@FirstName", member.FirstName);
                        command.Parameters.AddWithValue("@Party", member.Party);
                        command.Parameters.AddWithValue("@State", member.State);
                        command.Parameters.AddWithValue("@LisMemberId", member.LisMemberId);

                        Console.WriteLine($"Insert/update to Senator: ${member.FullName}, LisMemberId: ${member.LisMemberId}");
                        var result = command.ExecuteNonQuery();
                    }

                    foreach (RollCallVote vote in votes)
                    {
                        using SqlCommand command = connection.CreateCommand();
                        command.CommandText = $@"
                            BEGIN TRAN t1;
                            UPDATE dbo.LegislativeItems SET Title = @Title, VoteDate = @VoteDate, ModifyDate = @ModifyDate, VoteQuestionText = @VoteQuestionText, 
                                VoteDocumentText = @VoteDocumentText, MajorityRequirement = @MajorityRequirement, DocumentCongress = @DocumentCongress, 
                                DocumentType = @DocumentType, DocumentNumber = @DocumentNumber, DocumentName = @DocumentName, DocumentTitle = @DocumentTitle, 
                                DocumentShortTitle = @DocumentShortTitle, AmendmentNumber = @AmendmentNumber, AmendmentToDocumentNumber = @AmendmentToDocumentNumber,
                                AmendmentPurpose = @AmendmentPurpose, Issue = @Issue, IssueLink = @IssueLink, Question = @Question, Result = @Result, YeaCount = @YeaCount, 
                                NayCount = @NayCount, PresentCount = @PresentCount, AbsentCount = @AbsentCount, TieBreakerByWhom = @TieBreakerByWhom, TieBreakerVote = @TieBreakerVote
                            WHERE SessionID = @SessionID and VoteNumber = @VoteNumber;
                            
                            IF @@ROWCOUNT = 0
                            BEGIN
                                INSERT INTO dbo.LegislativeItems(SessionId, Title, VoteNumber, VoteDate, ModifyDate, VoteQuestionText, VoteDocumentText,
                                    MajorityRequirement, DocumentCongress, DocumentType, DocumentNumber, DocumentName, DocumentTitle, DocumentShortTitle,
                                    AmendmentNumber, AmendmentToDocumentNumber, AmendmentPurpose, Issue, IssueLink, Question, Result, YeaCount, NayCount,
                                    PresentCount, AbsentCount, TieBreakerByWhom, TieBreakerVote)
                                VALUES (@SessionId, @Title, @VoteNumber, @VoteDate, @ModifyDate, @VoteQuestionText,
                                    @VoteDocumentText, @MajorityRequirement, @DocumentCongress, @DocumentType, @DocumentNumber, @DocumentName,
                                    @DocumentTitle, @DocumentShortTitle, @AmendmentNumber, @AmendmentToDocumentNumber, @AmendmentPurpose, @Issue,
                                    @IssueLink, @Question, @Result, @YeaCount, @NayCount, @PresentCount, @AbsentCount, @TieBreakerByWhom, @TieBreakerVote)
                            END

                            COMMIT TRAN t1;
                            ";

                        command.Parameters.AddWithValue("@SessionID", currentSessionId);
                        command.Parameters.AddWithValue("@Title", vote.VoteTitle);
                        command.Parameters.AddWithValue("@VoteNumber", vote.VoteNumber);
                        command.Parameters.AddWithValue("@VoteDate", DateTime.TryParse(vote.VoteDate, out DateTime voteDate) ? voteDate : (object) DBNull.Value);
                        //command.Parameters.AddWithValue("@ModifyDate", vote.ModifyDate);
                        command.Parameters.AddWithValue("@ModifyDate", DBNull.Value);
                        string questionText = vote.VoteQuestionText?.Length > 500 ? vote.VoteQuestionText.Substring(0, 500) : vote.VoteQuestionText;
                        command.Parameters.AddWithValue("@VoteQuestionText", questionText);
                        command.Parameters.AddWithValue("@VoteDocumentText", vote.VoteDocumentText);
                        command.Parameters.AddWithValue("@MajorityRequirement", vote.MajorityRequirement);
                        //command.Parameters.AddWithValue("@DocumentCongress", vote.Document.Congress);
                        command.Parameters.AddWithValue("@DocumentCongress", DBNull.Value);
                        command.Parameters.AddWithValue("@DocumentType", vote.Document?.Type ?? (object) DBNull.Value);
                        command.Parameters.AddWithValue("@DocumentNumber", vote.Document?.Number ?? (object) DBNull.Value);
                        command.Parameters.AddWithValue("@DocumentName", vote.Document?.Name ?? (object) DBNull.Value);
                        command.Parameters.AddWithValue("@DocumentTitle", vote.Document?.Title ?? (object) DBNull.Value);
                        command.Parameters.AddWithValue("@DocumentShortTitle", vote.Document?.ShortTitle ?? (object) DBNull.Value);
                        command.Parameters.AddWithValue("@AmendmentNumber", vote.Amendment?.Number ?? (object) DBNull.Value);
                        command.Parameters.AddWithValue("@AmendmentToDocumentNumber", vote.Amendment?.ToDocumentNumber ?? (object) DBNull.Value);
                        command.Parameters.AddWithValue("@AmendmentPurpose", vote.Amendment?.Purpose ?? (object) DBNull.Value);
                        command.Parameters.AddWithValue("@Issue", DBNull.Value);
                        command.Parameters.AddWithValue("@IssueLink", DBNull.Value);
                        command.Parameters.AddWithValue("@Question", vote.Question);
                        string voteResult = vote.VoteResult?.Length > 250 ? vote.VoteResult.Substring(0, 250) : vote.VoteResult;
                        command.Parameters.AddWithValue("@Result", voteResult);
                        command.Parameters.AddWithValue("@YeaCount", vote.Count.Yeas);
                        command.Parameters.AddWithValue("@NayCount", vote.Count.Nays);
                        command.Parameters.AddWithValue("@PresentCount", vote.Count.Present);
                        command.Parameters.AddWithValue("@AbsentCount", vote.Count.Absent);
                        command.Parameters.AddWithValue("@TieBreakerByWhom", vote.TieBreaker.ByWhom);
                        command.Parameters.AddWithValue("@TieBreakerVote", vote.TieBreaker.TieBreakerVote);

                        Console.WriteLine($"Insert/update LegItem: session: {currentSessionId} voteNum: {vote.VoteNumber}");
                        int result = command.ExecuteNonQuery();
                    }

                    var legislativeItems = connection.Query < (int Id, int SessionID, string VoteNumber) > ($@"
                        SELECT Id, SessionID, VoteNumber 
                          FROM dbo.LegislativeItems
                         WHERE SessionID = @SessionID",
                        new { SessionID = currentSessionId }).ToDictionary(i => i.VoteNumber, i => i.Id);

                    var senators = connection.Query < (string LisMemberId, int Id) > ($"SELECT LisMemberId, Id from dbo.Senators")
                        .ToDictionary(i => i.LisMemberId, i => i.Id);

                    foreach (RollCallVote vote in votes)
                    {
                        if (!legislativeItems.TryGetValue(vote.VoteNumber, out int legislativeItemID))
                        {
                            legislativeItemID = -1;
                        }

                        var votesBySenatorId = connection.Query < (int Id, int SenatorId, int legislativeItemID, string VoteCast) > ($@"
                            SELECT * FROM dbo.Votes WHERE LegislativeItemID = @LegislativeItemID
                        ", new { LegislativeItemID = legislativeItemID }).ToDictionary(v => v.SenatorId, v => v);

                        var stopwatch = Stopwatch.StartNew();
                        foreach (Member member in vote.Members.MemberElements)
                        {
                            var senatorId = senators[member.LisMemberId];
                            if (votesBySenatorId.ContainsKey(senatorId))
                            {
                                //Console.WriteLine($"Skipping member vote({vote.VoteNumber}) for session: {vote.Congress}|{vote.Session}: {member.FullName}");
                                continue;
                            }

                            using SqlCommand command = connection.CreateCommand();
                            command.CommandText = $@"
                                INSERT INTO dbo.Votes VALUES (@SenatorId, @LegislativeItemId, @VoteCast);
                            ";

                            command.Parameters.AddWithValue("@SenatorId", senatorId);
                            command.Parameters.AddWithValue("@LegislativeItemId", legislativeItemID);
                            command.Parameters.AddWithValue("@VoteCast", member.VoteCast);

                            //Console.WriteLine($"Inserting member vote({vote.VoteNumber}) for session: {vote.Congress}|{vote.Session}: {member.FullName}");
                            int result = command.ExecuteNonQuery();
                        }

                        stopwatch.Stop();
                        Console.WriteLine($"Inserted ({stopwatch.ElapsedMilliseconds}) votes for ({vote.VoteNumber}), session: {vote.Congress}|{vote.Session}");
                    }
                }
            }
        }
    }
}