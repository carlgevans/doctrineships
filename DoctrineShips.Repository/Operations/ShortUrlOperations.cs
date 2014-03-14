namespace DoctrineShips.Repository.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DoctrineShips.Entities;
    using GenericRepository;

    internal sealed class ShortUrlOperations
    {
        private readonly IUnitOfWork unitOfWork;

        internal ShortUrlOperations(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal void DeleteShortUrl(string shortUrlId)
        {
            this.unitOfWork.Repository<ShortUrl>().Delete(shortUrlId);
        }

        internal void UpdateShortUrl(ShortUrl shortUrl)
        {
            shortUrl.ObjectState = ObjectState.Modified;
            this.unitOfWork.Repository<ShortUrl>().Update(shortUrl);
        }

        internal ShortUrl AddShortUrl(ShortUrl shortUrl)
        {
            this.unitOfWork.Repository<ShortUrl>().Insert(shortUrl);
            return shortUrl;
        }

        internal ShortUrl CreateShortUrl(ShortUrl shortUrl)
        {
            shortUrl.ObjectState = ObjectState.Added;
            this.unitOfWork.Repository<ShortUrl>().Insert(shortUrl);
            return shortUrl;
        }

        internal ShortUrl GetShortUrl(string shortUrlId)
        {
            return this.unitOfWork.Repository<ShortUrl>().Find(shortUrlId);
        }

        internal IEnumerable<ShortUrl> GetShortUrls()
        {
            var shortUrls = this.unitOfWork.Repository<ShortUrl>()
                              .Query()
                              .Get()
                              .OrderBy(x => x.DateCreated)
                              .ToList();

            return shortUrls;
        }

        internal void DeleteShortUrlsOlderThanDate(DateTime olderThanDate)
        {
            IEnumerable<ShortUrl> shortUrls = null;

            shortUrls = this.unitOfWork.Repository<ShortUrl>()
                      .Query()
                      .Filter(x => x.DateCreated < olderThanDate)
                      .Get()
                      .ToList();

            foreach (var item in shortUrls)
            {
                this.unitOfWork.Repository<ShortUrl>().Delete(item);
            }
        }
    }
}