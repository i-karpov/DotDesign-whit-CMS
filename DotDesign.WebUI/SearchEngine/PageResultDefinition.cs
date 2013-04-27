using DotDesign.Domain.Entities;
using Lucene.Net.Documents;
using SimpleLucene;

namespace DotDesign.WebUI.SearchEngine
{
    public class PageResultDefinition : IResultDefinition<Page>
    {
        public Page Convert(Document document)
        {
            return new Page
                       {
                           Id = document.GetValue<int>("Id"),
                           Title = document.GetValue("Title"),
                           Description = document.GetValue("Description")
                       };
        }
    }
}