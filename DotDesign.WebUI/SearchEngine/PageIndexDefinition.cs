using System;
using System.Globalization;
using DotDesign.Domain.Entities;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using SimpleLucene;

namespace DotDesign.WebUI.SearchEngine
{
    public class PageIndexDefinition : IIndexDefinition<Page>
    {
        Document IIndexDefinition<Page>.Convert(Page page)
        {
            Document document = new Document();

            document.Add(new Field("Id",
                                    page.Id.ToString(CultureInfo.InvariantCulture),
                                    Field.Store.YES,
                                    Field.Index.NOT_ANALYZED));
            document.Add(new Field("Title",
                                    page.Title ?? String.Empty,
                                    Field.Store.YES,
                                    Field.Index.ANALYZED));
            document.Add(new Field("Description",
                                    page.Description ?? String.Empty,
                                    Field.Store.YES,
                                    Field.Index.ANALYZED));
            document.Add(new Field("Contents",
                                    page.ContentsMarkup ?? String.Empty,
                                    Field.Store.NO,
                                    Field.Index.ANALYZED));

            return document;
        }

        Term IIndexDefinition<Page>.GetIndex(Page page)
        {
            return new Term("Id",
                            page.Id.ToString(CultureInfo.InvariantCulture));
        }
    }
}