//	Copyright © 2015 Alexander Isakov. All rights reserved.
//  This file is a part of Converter.NET and is licensed under MS-PL.
//  See LICENSE for details or visit http://opensource.org/licenses/MS-PL.
//	----------------------------------------------------------------------

using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.PrettyPrinter;
using ICSharpCode.NRefactory.Visitors;

namespace Converter.Net
{
    public static class Converter
    {
        public static string ConvertCS2VBNET(string cscode)
        {
            var snippetParser = new SnippetParser(SupportedLanguage.CSharp);
            var node = snippetParser.Parse(cscode);
            node.AcceptVisitor(new ToVBNetConvertVisitor(), null);
            var netOutputVisitor = new VBNetOutputVisitor();
            using (SpecialNodesInserter.Install(snippetParser.Specials, netOutputVisitor))
                node.AcceptVisitor(netOutputVisitor, null);
            return netOutputVisitor.Text;
        }

        public static string ConvertVBNET2CS(string vbcode)
        {
            var snippetParser = new SnippetParser(SupportedLanguage.VBNet);
            var node = snippetParser.Parse(vbcode);
            node.AcceptVisitor(new ToVBNetConvertVisitor(), null);
            var csharpOutputVisitor = new CSharpOutputVisitor();
            using (SpecialNodesInserter.Install(snippetParser.Specials, csharpOutputVisitor))
                node.AcceptVisitor(csharpOutputVisitor, null);
            return csharpOutputVisitor.Text;
        }
    }
}
