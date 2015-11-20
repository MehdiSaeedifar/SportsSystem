using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.PersianCaptcha
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ValidateCaptchaAttribute : ActionFilterAttribute
    {
        #region Fields (7)

        /// <summary>
        /// پیغامی مبنی بر اینکه در رمزگشایی اطلاعات کوکی تصویر امنیتی خطایی رخ داده است
        /// </summary>
        public string ErrorWasHappened { get; set; }

        /// <summary>
        /// پیغام مبنی بر این که وارد کردن کد تصویر امنیتی الزامی است
        /// </summary>
        public string CaptchaCodeIsRequired { get; set; }

        /// <summary>
        /// کد امنیتی وارد شده توسط کاربر صحیح نمی باشد
        /// </summary>
        public string CaptchaCodeIsIncorrect { get; set; }

        /// <summary>
        ///  پیغام مبنی بر این که زمان ارسال فرم حاوی تصویر امنیتی به پایان رسیده است
        /// </summary>
        public string TimeIsExpired { get; set; }

        /// <summary>
        /// پیغام مبنی بر اینکه کوکی مرورگر باید فعال باشد
        /// </summary>
        public string CookieMustEnabled { get; set; }

        /// <summary>
        /// حداکثر زمان ممکن برای ارسال فرم حاوی تصویر امنیتی
        /// </summary>
        public int ExpireTimeCaptchaCodeBySeconds { get; set; }

        /// <summary>
        /// کلید رمزگشایی اطلاعات، این کلید باید با کلید رمزنگاری اطلاعات که در کلاس
        /// CaptchaImageResult
        /// تعریف شده است یکسان باشد
        /// </summary>
        private const string DecryptionKey = "jd23_=sd23liowe|23aotq";

        #endregion

        #region Ctors (2)

        public ValidateCaptchaAttribute()
        {
            ErrorWasHappened = "خطایی اتفاق افتاده است";
            CaptchaCodeIsRequired = "لطفا کد امنیتی را وارد کنید";
            TimeIsExpired = "حداکثر مهلت وارد کردن کد امنیتی تمام شده است";
            CaptchaCodeIsIncorrect = "کد امنیتی را اشتباه وارد کرده اید";
            CookieMustEnabled = "باید ابتدا قابلیت کوکی ها را در مرورگر خود فعال کنید";
            ExpireTimeCaptchaCodeBySeconds = 120;
        }

        public ValidateCaptchaAttribute(string errorWasHappened, string captchaCodeIsRequired, string captchaCodeIsIncorrect,
            string timeIsExpired, string cookieMustEnabled, int expireTimeCaptchaCodeBySeconds)
        {
            this.ErrorWasHappened = errorWasHappened;
            this.CaptchaCodeIsRequired = captchaCodeIsRequired;
            this.CaptchaCodeIsIncorrect = captchaCodeIsIncorrect;
            this.TimeIsExpired = timeIsExpired;
            this.CookieMustEnabled = cookieMustEnabled;
            this.ExpireTimeCaptchaCodeBySeconds = expireTimeCaptchaCodeBySeconds;
        }

        #endregion

        #region Methods (1)
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerBase = filterContext.Controller;

            var captchaInputTextProvider = controllerBase.ValueProvider.GetValue("CaptchaInputText");
            if (captchaInputTextProvider == null)
            {
                controllerBase.ViewData.ModelState.AddModelError("CaptchaInputText", ErrorWasHappened);
                base.OnActionExecuting(filterContext);
                return;
            }
            var inputText = captchaInputTextProvider.AttemptedValue;

            var httpCookie = HttpContext.Current.Request.Cookies["captchastring"];

            if (httpCookie == null)
            {
                controllerBase.ViewData.ModelState.AddModelError("CaptchaInputText", CookieMustEnabled);
                return;
            }

            string decryptedString = "";
            try
            {
                //-- رمزگشایی کردن مقدار کوکی
                //-- با توجه به این که تاریخ فعلی (فقط روز فعلی، زمان نباید اضافه شود) موقع رمزنگاری به کلید رمزنگاری اضافه گردید
                //-- باز هم تاریخ فعلی (فقط روز فعلی، زمان نباید اضافه شود) به کلید 
                //-- رمزگشایی اضافه گردیده است
                decryptedString = httpCookie.Value.Decrypt(DecryptionKey + DateTime.Now.Date.ToString(CultureInfo.InvariantCulture));
            }
            catch (System.Security.Cryptography.CryptographicException exception)
            {
                //-- خطایی در رمزگشایی اطلاعات اتفاق افتاده است، به بیانی دیگر کسی سعی در هک کردن کوکی را داشته
                //--  یا این که یک یا بیش از یک روز از زمان ارسال فرم از سرور به مرورگر کاربر گذشته است 
                //--  و کاربر الآن فرم را به سرور ارسال کرده است

                //-- وجود خط زیر ضروری است، در غیر این صورت مهاجم به راحتی عملیات را انجام می دهد، بدون این که
                //-- کد امنیتی را درست وارد کرده باشد، البته نوع خطا را نباید به او نشان دهید، این خطا باید در
                //-- جایی لاگ شود تا مدیر سایت بتواند آنرا بررسی کند
                controllerBase.ViewData.ModelState.AddModelError("CaptchaInputText", ErrorWasHappened);
                return;
            }

            string[] arr = decryptedString.Split(',');
            string originalCaptchaNumber = arr[0];
            string generatedCaptchaDateTime = arr[1];

            int num;
            DateTime dt = new DateTime();
            
            
            if (originalCaptchaNumber == "" || generatedCaptchaDateTime == "" || !int.TryParse(originalCaptchaNumber, out num) ||
                !DateTime.TryParse(generatedCaptchaDateTime,new CultureInfo("en"),DateTimeStyles.AssumeUniversal, out dt))
            {
                //-- اطلاعات رمزگشایی شده معتبر نیستند

                //-- وجود خط زیر ضروری است، در غیر این صورت مهاجم به راحتی عملیات را انجام می دهد، بدون این که
                //-- کد امنیتی را درست وارد کرده باشد، البته نوع خطا را نباید به او نشان دهید، این خطا باید در
                //-- جایی لاگ شود تا مدیر سایت بتواند آنرا بررسی کند
                controllerBase.ViewData.ModelState.AddModelError("CaptchaInputText", ErrorWasHappened);
                return;
            }


            //-- به دست آوردن اختلاف زمانی بر حسب ثانیه، بین موقعی که تصویر امنیتی ایجاد شد و زمان فعلی که کاربر 
            //-- کد امنیتی را وارد کرده و فرم را پست کرده است
            double secondsDiff = (DateTime.Now - DateTime.Parse(generatedCaptchaDateTime)).TotalSeconds;


            if (secondsDiff > ExpireTimeCaptchaCodeBySeconds) //-- اگر بیشتر از 30 ثانیه طول کشیده باشد تا فرم حاوی تصویر امنیتی پست شود
            {
                controllerBase.ViewData.ModelState.AddModelError("CaptchaInputText", TimeIsExpired);
                return;
            }

            if (inputText != originalCaptchaNumber)
            {
                controllerBase.ViewData.ModelState.AddModelError("CaptchaInputText", CaptchaCodeIsIncorrect);
                return;
            }

            HttpContext.Current.Response.Cookies.Remove("captchastring");
        }

        #endregion
    }
}