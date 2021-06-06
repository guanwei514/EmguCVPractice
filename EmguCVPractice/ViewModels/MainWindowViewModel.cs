using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EmguCVPractice.Extensions;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Interop;
using System.Windows;

namespace EmguCVPractice.ViewModels
{
    public class MainWindowViewModel: ViewModelBase, IDisposable
    {
        public MainWindowViewModel()
        {

        }
        #region ViewLoaded
        public ICommand ViewLoaded { get { return new RelayCommand(param => ViewLoadedExecute(), param => CanViewLoadedExectue()); } }
        private void ViewLoadedExecute()
        {
            //要使用EmguCV的功能，大多都要先透過CvInvoke去做。
            //Imread:Image Read
            //Imwrite: Image Write
            //Imshow: Image Show,跳出一個視窗顯示這張圖片
            ////Image方法
            //Image<Bgr, Byte> img = new Image<Bgr, byte>(320, 240, new Bgr(255, 0, 0));
            ////Mat方法 
            //Mat img = new Mat(200, 400, DepthType.Cv8U, 3);
            //img.SetTo(new Bgr(255, 0, 0).MCvScalar);
            ////cvCreateImage方法
            //IntPtr img = CvInvoke.cvCreateImage(CvInvoke.cvGetSize(scr),
            //Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_8U, 1);
            //CvInvoke.cvCopy(scr, img, IntPtr.Zero);
            //ImageBox.Image = img;

            string serverDir = System.IO.Path.GetFullPath(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..\\..\\Images\\"));
            _Img = CvInvoke.Imread(serverDir +"lena.jpg");
            MyImage = ImageSourceFromBitmap(_Img.ToImage<Bgr, byte>().ToBitmap());
        }
        private bool CanViewLoadedExectue()
        {
            return true;
        }
        #endregion
        #region ViewModelDispose
        public void Dispose()
        {

        }
        #endregion
        #region Properties
        private Mat _Img = new Mat();
        private ImageSource _MyImage = null;
        public ImageSource MyImage { get { return _MyImage; } set { _MyImage = value; NotifyPropertyChanged(); } }
        #endregion
        #region ToGrayScaleButtonClick
        public ICommand ToGrayScaleButtonClick { get { return new RelayCommand(param => ToGrayScaleButtonClickExecute(), param => true); } }
        private void ToGrayScaleButtonClickExecute()
        {
            Mat grayImg = new Mat();
            CvInvoke.CvtColor(_Img, grayImg, ColorConversion.Rgb2Gray);
            CvInvoke.Imshow("GrayImg", grayImg);
        }
        #endregion

        #region Methods
        //If you get 'dllimport unknown'-, then add 'using System.Runtime.InteropServices;'
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }
        #endregion
    }
}
