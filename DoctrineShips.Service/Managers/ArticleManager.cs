namespace DoctrineShips.Service.Managers
{
    using System.Collections.Generic;
    using DoctrineShips.Entities;
    using DoctrineShips.Repository;
    using DoctrineShips.Validation;
    using Tools;

    /// <summary>
    /// A class dealing with Doctrine Ships articles.
    /// </summary>
    internal sealed class ArticleManager
    {
        private readonly IDoctrineShipsRepository doctrineShipsRepository;
        private readonly IDoctrineShipsValidation doctrineShipsValidation;
        private readonly ISystemLogger logger;

        /// <summary>
        /// Initialises a new instance of an Article Manager.
        /// </summary>
        /// <param name="doctrineShipsRepository">An IDoctrineShipsRepository instance.</param>
        /// <param name="doctrineShipsValidation">An IDoctrineShipsValidation instance.</param>
        /// <param name="logger">An ISystemLogger logger instance.</param>
        internal ArticleManager(IDoctrineShipsRepository doctrineShipsRepository, IDoctrineShipsValidation doctrineShipsValidation, ISystemLogger logger)
        {
            this.doctrineShipsRepository = doctrineShipsRepository;
            this.doctrineShipsValidation = doctrineShipsValidation;
            this.logger = logger;
        }

        /// <summary>
        /// Fetches and returns a Doctrine Ships article.
        /// </summary>
        /// <param name="articleId">The id of the article for which an article object should be returned.</param>
        /// <returns>An article object.</returns>
        internal Article GetArticle(int articleId)
        {
            return this.doctrineShipsRepository.GetArticle(articleId);
        }

        /// <summary>
        /// Get a list of available articles. Articles with the 'IsUnlisted' flag will be omitted by default.
        /// </summary>
        /// <param name="includeUnlisted">A flag to indicate if articles marked as 'IsUnlisted' should be returned.</param>
        /// <returns>Returns a list of article objects.</returns>
        internal IEnumerable<Article> GetArticles(bool includeUnlisted = false)
        {
            return this.doctrineShipsRepository.GetArticles(includeUnlisted);
        }
    }
}
