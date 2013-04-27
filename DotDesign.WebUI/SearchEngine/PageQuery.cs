
using System;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using SimpleLucene.Impl;

namespace DotDesign.WebUI.SearchEngine
{
    public class PageQuery : QueryBase
    {
        /// <summary>
        /// Fields specify in wich terms search will be executed.
        /// </summary>
        private static readonly String[] Fields = {
                                                       "Title",
                                                       "Description",
                                                       "Contents"
                                                   };

        public PageQuery(Query query) : base(query)
        {
        }

        public PageQuery()
        {        
        }

        public PageQuery WithKeywords(String keywords)
        {
           if (!String.IsNullOrEmpty(keywords))
           {
               QueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29,
                               Fields,
                               new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
               Query multiQuery = parser.Parse(keywords);
               this.AddQuery(multiQuery);
           }
           return this;
        }
    }
}