using MdConv.Lib;

string mddoc = File.ReadAllText(@"E:\Repo\TechDown\MarkdownTest.md");
File.WriteAllText(@"E:\TestData\mdgen.html", Converter.ToHTML(mddoc, "TEST DOCUMENT", "?", true));
