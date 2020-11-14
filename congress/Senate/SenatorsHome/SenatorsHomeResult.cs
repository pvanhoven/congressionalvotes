using System.Collections.Generic;
using congress.Model;

public record SenatorsHomeResult(
    IEnumerable<Senator> CurrentSessionSenators,
    int CurrentSessionId,
    IEnumerable<Session> AvailableSessions
);