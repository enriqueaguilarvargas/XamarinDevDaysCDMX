using System;
using UIKit;
using System.Drawing;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
namespace MVAV41
{
	public partial class ViewController : UIViewController
	{
		string archivoLocal;
		AVCaptureDevice dispositivodeCaptura;
		AVCaptureSession sesiondeCaptura;
		AVCaptureDeviceInput entradaDispositivo;
		AVCaptureStillImageOutput salidaImagen;
		AVCaptureVideoPreviewLayer preview;
		string ruta;
		byte[] arregloJpg;
		protected ViewController(IntPtr handle) : base(handle)
		{
		}
		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();
			await autorizacionCamara();
			ConfiguracionCamara();
			btnCapturar.TouchUpInside += async delegate
			{
				var salidadevideo = salidaImagen.ConnectionFromMediaType(AVMediaType.Video);
				var bufferdevideo = await salidaImagen.CaptureStillImageTaskAsync(salidadevideo);
				var datosImagen = AVCaptureStillImageOutput.JpegStillToNSData(bufferdevideo);

				arregloJpg = datosImagen.ToArray();
				string rutacarpeta = Environment.GetFolderPath
												  (Environment.SpecialFolder.Personal);
				string resultado = "Foto";
				archivoLocal = resultado + ".jpg";
				ruta = Path.Combine(rutacarpeta, archivoLocal);
				File.WriteAllBytes(ruta, arregloJpg);
				Imagen.Image = UIImage.FromFile(ruta);
			};
			btnRespaldar.TouchUpInside += async delegate
			{
				try
				{
					CloudStorageAccount cuentaAlmacenamiento = CloudStorageAccount.Parse
						("DefaultEndpointsProtocol=https;AccountName=enriqueaguilar;AccountKey=CLAVEAPIAZURESTORAGE");
					CloudBlobClient clienteBlob = cuentaAlmacenamiento.CreateCloudBlobClient();
					CloudBlobContainer contenedor = clienteBlob.GetContainerReference("imagenes");
					CloudBlockBlob recursoblob = contenedor.GetBlockBlobReference(archivoLocal);
					await recursoblob.UploadFromFileAsync(ruta);
					MessageBox("Guardado en", "Azure Storage - Blob");
				}
				catch (StorageException ex)
				{
					MessageBox("Error: ", ex.Message);
				}
			};
		}
		async Task autorizacionCamara()
		{
			var estatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
			if (estatus != AVAuthorizationStatus.Authorized)
			{
				await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
			}
		}
		public void ConfiguracionCamara()
		{
			sesiondeCaptura = new AVCaptureSession();
			preview = new AVCaptureVideoPreviewLayer(sesiondeCaptura)
			{
				Frame = new RectangleF(40, 50, 300, 350)
			};
			View.Layer.AddSublayer(preview);
			dispositivodeCaptura = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
			entradaDispositivo = AVCaptureDeviceInput.FromDevice(dispositivodeCaptura);
			sesiondeCaptura.AddInput(entradaDispositivo);
			salidaImagen = new AVCaptureStillImageOutput()
			{
				OutputSettings = new NSDictionary()
			};
			sesiondeCaptura.AddOutput(salidaImagen);
			sesiondeCaptura.StartRunning();
		}
		public static void MessageBox(string Title, string message)
		{
			var Alerta = new UIAlertView();
			Alerta.Title = Title;
			Alerta.Message = message;
			Alerta.AddButton("OK");
			Alerta.Show();
		}
	}
}