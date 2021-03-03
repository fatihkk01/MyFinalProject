using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CrossCuttingConcerns.Caching
{
    public interface ICacheManager
    {
        //Key : cache adı
        //Value : cache değeri
        //Duration : cache in kullanım süresi
        //Key ile bellekten kullanıcının verdiği keye karşılık gelen cache getirilir.
        T Get<T>(string key);
        object Get(string key);
        void Add(string key, object value, int duration);
        bool IsAdd(string key);
        void Remove(string key);
        //REmoveByPattern : Cache i silerken neye göre sileceğini belirleyen bir methottur (Methot adı içinde Category geçenler vs.)
        void RemoveByPattern(string pattern);
    }
}
