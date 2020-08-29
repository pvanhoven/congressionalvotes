using System.Collections.Generic;
using Oakton;

public class Options
{
    private static readonly int[] DefaultCongresses = new [] { 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116 };

    [Description("Congresses to process")]
    public IEnumerable<int> CongressesFlag { get; set; } = DefaultCongresses;

    [Description("Sessions to process")]
    public IEnumerable<int> SessionsFlag { get; set; } = new [] { 1, 2 };
}