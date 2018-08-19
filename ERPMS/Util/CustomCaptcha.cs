using BotDetect.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Util
{
    public class CustomCaptcha
    {

     
      
        public static string CurrentInstanceId { get { return currentInstanceId; } }
        private static string currentInstanceId;
        public static MvcCaptcha GetCaptcha(string captchaId)
        {
            MvcCaptcha exampleCaptcha = new MvcCaptcha(captchaId);
            exampleCaptcha.ImageStyle = BotDetect.ImageStyle.CaughtInTheNet2;
            exampleCaptcha.ReloadEnabled = false;
            exampleCaptcha.SoundEnabled = false;
            currentInstanceId = exampleCaptcha.CurrentInstanceId;
            return exampleCaptcha;
        }
        /// <summary>
        /// 获取自定义的验证码样式
        /// </summary>
        /// <param name="captchaId"></param>
        /// <param name="width">指定验证码图片的宽度</param>
        /// <param name="height">指定验证码图片的高度</param>
        /// <param name="codeLength">指定验证码的个数</param>
        /// <returns></returns>
        public static MvcCaptcha GetCaptcha(string captchaId, int width, int height, int codeLength)
        {
            MvcCaptcha exampleCaptcha = new MvcCaptcha(captchaId);
            exampleCaptcha.ImageStyle = BotDetect.ImageStyle.CaughtInTheNet2;//我个人喜欢的风格
            exampleCaptcha.ReloadEnabled = false;//去掉刷新的按钮
            exampleCaptcha.SoundEnabled = false;//去掉声音播放按钮
            exampleCaptcha.CodeLength = codeLength;//指定验证码的长度
            exampleCaptcha.ImageSize = new System.Drawing.Size(width, height);//指定图片的大小
            currentInstanceId = exampleCaptcha.CurrentInstanceId;//当前实例的id
            return exampleCaptcha;
        }
    }
}