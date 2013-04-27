using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.Hosting;
using DotDesign.Domain.Entities;
using SimpleLucene;
using SimpleLucene.Impl;
using SimpleLucene.IndexManagement;

namespace DotDesign.WebUI.SearchEngine
{
    public static class PageIndexHelper
    {
        private static readonly String IndexDirectoryPath = 
                   Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Index");
       
        private static readonly DirectoryInfo IndexDirectoryInfo = new DirectoryInfo(IndexDirectoryPath);

        public static void AddOrUpdate(Page page)
        {
            EntityUpdateTask<Page> entityUpdateTask = CreateEntityUpdateTask(page);
            using (IndexService indexService = new IndexService(
                                                     new DirectoryIndexWriter(IndexDirectoryInfo)))
            {
                entityUpdateTask.Execute(indexService);
            }
        }

        public static void Delete(int pageId)
        {
            EntityDeleteTask<Page> entityDeleteTask = CreateEntityDeleteTask(pageId);
            using (IndexService indexService = new IndexService(
                                                     new DirectoryIndexWriter(IndexDirectoryInfo)))
            {
                entityDeleteTask.Execute(indexService);
            }
        }

        public static IEnumerable<Page> Find(String searchText)
        {
            if (!Directory.Exists(IndexDirectoryPath))
            {
                return new List<Page>();
            }
            DirectoryIndexSearcher indexSearcher = new DirectoryIndexSearcher(IndexDirectoryInfo);
            using (SearchService searchService = new SearchService(indexSearcher))
            {
                QueryBase query = new PageQuery().WithKeywords(searchText);
                SearchResult<Page> result = searchService.
                                                SearchIndex(query.Query,
                                                            new PageResultDefinition());
                return result.Results;
            }
        }

        private static EntityUpdateTask<Page> CreateEntityUpdateTask(Page page)
        {
            FileSystemIndexLocation indexLocation = new FileSystemIndexLocation(IndexDirectoryInfo);
            PageIndexDefinition definition = new PageIndexDefinition();
            EntityUpdateTask<Page> entityUpdateTask = new EntityUpdateTask<Page>(page, definition, indexLocation);

            IndexQueue.Instance.Queue(entityUpdateTask);
            return entityUpdateTask;
        }

        private static EntityDeleteTask<Page> CreateEntityDeleteTask(int pageId)
        {
            FileSystemIndexLocation indexLocation = new FileSystemIndexLocation(IndexDirectoryInfo);
            EntityDeleteTask<Page> entityUpdateTask = 
                 new EntityDeleteTask<Page>(indexLocation,
                                            "Id",
                                            pageId.ToString(CultureInfo.InvariantCulture));

            IndexQueue.Instance.Queue(entityUpdateTask);
            return entityUpdateTask;
        }

    }
}