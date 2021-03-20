using Business.Abstract;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.Utilities.Business;
using Business.BusinessAspects.Autofac;
using Core.Aspects.Autofac.Caching;
using System.Transactions;
using Core.Aspects.Autofac.Transaction;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;

        //Not : Bir entity manager kendisi hariç başka bir manager enjekte edemez.
        public ProductManager(IProductDal productDal,ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }


        //[SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {
            //Encryption , Hashing
            //MD5,SHA1
            //Rainbow Table
            //Salting
            //Encrypt Decrypt
            //Claim

            //business codes (Ürünün eklenmesi için gerekli olan şartlar.)            
            //validation
            //fluent validation

            //Cross Cutting Concerns 

            //Logging
            //CacheRemove
            //Performance
            //Transaction
            //Authorization

            //business codes

            IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName),
                                             CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                                             CheckIfCategoryLimitExceded());

            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);
        }

        //Cache : Daha önceden yapılan işlemlerin bir sonraki çalıştırılmasında cache den yapılmasını sağlar.
        //Key-Value : Key cache e verilen isim value ise cache nin değeri.
        [CacheAspect]
        public IDataResult<List<Product>> GetAll()
        {
            //İş Kodları
            //Yetkisi var mı?
            //MaintenanceTime = Bakım Zamanı
            if (DateTime.Now.Hour == 5)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);

        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        [CacheAspect]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")] // Bellekteki tüm getleri iptal et
        public IResult Update(Product product)
        {

            IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName),
                                             CheckIfProductCountOfCategoryCorrect(product.CategoryId));

            if (result != null)
            {
                return result;
            }

            _productDal.Update(product);

            return new SuccessResult(Messages.ProductAdded);
        }

        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            // select count(*) from products where categoryId=1
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }

            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            // select count(*) from products where categoryId=1
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }

            return new SuccessResult();
        }

        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();

            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }

            return new SuccessResult();
        }

        //Nested Transaction araştır.
        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Product product)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                try
                {
                    Add(product);
                    if (product.UnitPrice < 10)
                    {
                        throw new Exception();
                    }
                    Add(product);
                    //Başarılı olursa scopeu complete et
                    scope.Complete();

                }
                catch (Exception)
                {
                    //Başarısız olursa Dispose et
                    scope.Dispose();
                }
            }
            
            return null;
        }
    }
}
