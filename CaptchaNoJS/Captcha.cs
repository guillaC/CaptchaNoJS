using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;

namespace CaptchaNoJS
{
    public class Captcha
    {
        private ICanvas canvas;
        private String answer;
        private Random random;
        private SkiaBitmapExportContext bmp;

        ///<summary>Init the captcha class
        ///<example>For example, we can instantiate this class like this:
        ///<code>
        ///Captcha cptch = new CaptchaNoJS.Captcha(350, 50, 15);
        ///</code>
        ///</example>
        ///</summary>

        public Captcha(int pWidth, int pHeight, int pLength)
        {
            bmp = new(pWidth, pHeight, 1.0f);
            canvas = bmp.Canvas;
            random = new Random();

            answer = GetRandomString(pLength);
        }

        /// <summary>  
        /// return captcha as base64 string
        /// <example>For example, we can use it in .cshtml razor view like this:
        /// <code>
        ///   img src="data:image/png;base64, @ViewData["captcha"]
        /// </code>
        /// <see href="https://goonlinetools.com/html-viewer/#jaii4893duk2b3ztkcem">see the result</see>
        /// </example>
        /// </summary>  
        public string GenerateAsB64(CaptchaType captchaType = CaptchaType.Line)
        {
            MemoryStream result = new MemoryStream();
            canvas.FillColor = GetRandomColor();

            switch (captchaType)
            {
                case CaptchaType.Line:
                    AddLines(30);
                    break;
                case CaptchaType.Circle:
                    AddCircles(30);
                    break;
                case CaptchaType.Random:

                    if (random.Next(0, 100) > 50)
                    {
                        AddLines(30);
                    }
                    else
                    {
                        AddCircles(300);
                    }
                    break;
            }

            canvas = AddStringToImg(canvas);
            bmp.WriteToStream(result);
            return Convert.ToBase64String(result.ToArray());
        }

        private void AddLines(int nb)
        {
            for (int i = 0; i < nb; i++)
            {
                canvas.StrokeColor = GetRandomColor();
                canvas.StrokeSize = random.Next(2, 10);
                canvas.DrawLine(random.Next(bmp.Width), random.Next(bmp.Height), random.Next(bmp.Width), random.Next(bmp.Height));
            }
        }
        private void AddCircles(int nb)
        {
            for (int i = 0; i < nb; i++)
            {
                canvas.StrokeColor = GetRandomColor();
                canvas.StrokeSize = random.Next(2, 10);
                canvas.DrawCircle(new PointF(random.Next(bmp.Width), random.Next(bmp.Height)), random.Next(2, 20));
            }
        }

        /// <summary>  
        /// return the correct answer
        /// </summary>

        public String GetAnswer()
        {
            return answer;
        }

        private ICanvas AddStringToImg(ICanvas pCanvas)
        {
            PointF strLoc = new PointF(20f, 20f);

            for (int i = 0; i < answer.Length; i++)
            {
                pCanvas.StrokeColor = Colors.White;
                pCanvas.FontColor = GetRandomColor();
                pCanvas.Font = new Font("Arial");
                pCanvas.FontSize = random.Next(20, 25);
                pCanvas.DrawString(answer[i].ToString(), strLoc.X, strLoc.Y, HorizontalAlignment.Right);

                strLoc.X += (bmp.Width / answer.Length);
                strLoc.Y = random.Next(20, bmp.Height - 5);
            }
            return pCanvas;
        }
        private string GetRandomString(int length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private Color GetRandomColor()
        {
            return new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        }

        public enum CaptchaType
        {
            Line,
            Circle,
            Random
        }
    }
}
