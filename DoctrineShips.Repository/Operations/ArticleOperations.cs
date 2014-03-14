namespace DoctrineShips.Repository.Operations
{
    using System.Collections.Generic;
    using System.Linq;
    using DoctrineShips.Entities;
    using GenericRepository;

    internal sealed class ArticleOperations
    {
        private readonly IUnitOfWork unitOfWork;

        internal ArticleOperations(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal void DeleteArticle(int articleId)
        {
            this.unitOfWork.Repository<Article>().Delete(articleId);
        }

        internal void UpdateArticle(Article article)
        {
            article.ObjectState = ObjectState.Modified;
            this.unitOfWork.Repository<Article>().Update(article);
        }

        internal Article AddArticle(Article article)
        {
            this.unitOfWork.Repository<Article>().Insert(article);
            return article;
        }

        internal Article CreateArticle(Article article)
        {
            article.ObjectState = ObjectState.Added;
            this.unitOfWork.Repository<Article>().Insert(article);
            return article;
        }

        internal Article GetArticle(int articleId)
        {
            return this.unitOfWork.Repository<Article>().Find(articleId);
        }

        internal IEnumerable<Article> GetArticles(bool includeUnlisted = false)
        {
            var articles = this.unitOfWork.Repository<Article>()
                              .Query()
                              .Filter(q => q.IsUnlisted == includeUnlisted)
                              .Get()
                              .OrderBy(x => x.Title)
                              .ToList();

            return articles;
        }
    }
}