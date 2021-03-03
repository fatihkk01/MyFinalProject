using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            //MemoryCache ın karşılığı AddMemoryCache() olacak
            //Arka planda CacheManager instance ı oluşturur.
            serviceCollection.AddMemoryCache();
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //Bunu yapmamızın nedeni yarın cache sistemi olarak redise geçersek sistemin kolayca geçiş yapabilmesini sağlamak
            serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();
        }
    }
}
