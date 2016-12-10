using Android.App;
using Android.Widget;
using Android.OS;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
namespace AndroidDescargaImagen
{
	[Activity(Label = "AndroidDescargaImagen", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		ImageView Imagen;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Main);
			Button btnImagen = FindViewById<Button>(Resource.Id.btnBajar);
			Imagen = FindViewById<ImageView>(Resource.Id.Imagen);
			btnImagen.Click += async delegate
			{
				try
				{
					string carpeta = System.Environment.GetFolderPath
						(System.Environment.SpecialFolder.Personal);
					string archivoLocal = "Foto.jpg";
					string ruta = System.IO.Path.Combine(carpeta, archivoLocal);
					CloudStorageAccount cuentaAlmacenamiento = CloudStorageAccount.Parse
			("DefaultEndpointsProtocol=https;AccountName=enriqueaguilar;AccountKey=CLAVEAPIAZURESTORAGE");
					CloudBlobClient clienteBlob = cuentaAlmacenamiento.CreateCloudBlobClient();
					CloudBlobContainer contenedor = clienteBlob.GetContainerReference("imagenes");
					CloudBlockBlob recursoBlob = contenedor.GetBlockBlobReference("Foto.jpg");
					var stream = File.OpenWrite(ruta);
					await recursoBlob.DownloadToStreamAsync(stream);
					Android.Net.Uri rutaImagen = Android.Net.Uri.Parse(ruta);
					Imagen.SetImageURI(rutaImagen);
				}
				catch (StorageException ex)
				{
					Toast.MakeText(this, ex.Message, ToastLength.Short);
				}
			};
		}
	}
}
