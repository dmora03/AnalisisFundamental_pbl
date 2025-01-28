using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF.Behaviors
{
    /// <summary>
    /// Clase usada para tener una imagen capaz de cambiar a gris cuando el control no esta habilitado.
    /// </summary>
    public class AutoDisableImage : Image
    {
        /// <summary>
        /// Propiedad que indica si la imagen esta en escala de grises o no
        /// </summary>
        protected bool IsGrayscaled => Source is FormatConvertedBitmap;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="AutoDisableImage"/>.
        /// </summary>
        static AutoDisableImage()
        {
            // Override the metadata of the IsEnabled and Source properties to be notified of changes
            IsEnabledProperty.OverrideMetadata(typeof(AutoDisableImage), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnAutoDisableImagePropertyChanged)));
            SourceProperty.OverrideMetadata(typeof(AutoDisableImage), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnAutoDisableImagePropertyChanged)));
        }
        /// <summary>
        /// Se llama cuando cambian los valores de la propiedad IsEnabled o Source de <see cref="AutoDisableImage"/>
        /// </summary>
        /// <param name="source">La fuente.</param>
        /// <param name="args">La instancia <see cref="DependencyPropertyChangedEventArgs"/> que contiene los datos del evento.</param>
        private static void OnAutoDisableImagePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            if (source is AutoDisableImage autoDisableImage &&
                autoDisableImage.IsEnabled == autoDisableImage.IsGrayscaled)
            {
                if (autoDisableImage.Source == null) { return; }

                if (autoDisableImage.IsEnabled)     // If image is enabled then use the original image
                {
                    if (autoDisableImage.IsGrayscaled)
                    {
                        // Restore the Source property to the original value.
                        autoDisableImage.Source = ((FormatConvertedBitmap)autoDisableImage.Source).Source;
                        // Reset the Opcity Mask
                        autoDisableImage.OpacityMask = null;
                    }
                }
                else
                {
                    if (!autoDisableImage.IsGrayscaled)
                    {
                        // Get the source bitmap
                        BitmapImage? bitmapImage = new(new Uri(autoDisableImage.Source.ToString()));
                        // Convert it to Gray
                        autoDisableImage.Source = new FormatConvertedBitmap(bitmapImage, PixelFormats.Gray32Float, null, 0);
                        // Create Opacity Mask for greyscale image as FormatConvertedBitmap does not keep transparency info
                        autoDisableImage.OpacityMask = new ImageBrush(bitmapImage);

                        /* Si el codigo anterior da error por el TYPE que no es BITMAPSOURCE usar el siguiente codigo en su lugar/
                        // Get the source bitmap if Image Source is of type BitmapSource       
                        if (autoDisableImage.Source is BitmapSource bitmapImage)
                        {
                            // Convert it to Gray
                            autoDisableImage.Source = new FormatConvertedBitmap(bitmapImage, PixelFormats.Gray32Float, null, 0);
                            // reuse the opacity mask from the original image as FormatConvertedBitmap does not keep transparency info
                            autoDisableImage.OpacityMask = new ImageBrush(bitmapImage);
                        }/**/
                    }
                }
            }
        }
    }
}

/* COMO USAR

    1.- Definir el namespace
            xmlns:be="clr-namespace:WPF.Behaviors"

    2.- Usarlo como un Objeto nuevo
            <Button Grid.Column="1">
                <be:AutoDisableImage Source="Images/AddCompany.png"/>
            </Button>
 */