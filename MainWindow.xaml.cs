using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//**********************************************
//Bugs to fix: 
//Things to add: Keyboard control
// Change Color
//
//**********************************************
namespace ImageGallery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        int z = 0; //Control variable for Array
        int currentimagenum = 0;
        
        int FolderCount;
        string Point;
        string Location; //Absolute path of directory
        string SaveLocation;
        string ImageName;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Back Button
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            
            if (Location == null) //Checks to see if a directory has been selected.
            {
                System.Windows.MessageBox.Show("You must Enter a Location!");
                return;
            }

            string[] extensions = { ".jpg", ".png", ".jpeg", ".gif" };

            string[] filepaths2 = Directory.GetFiles(Location, "*.*") //Array that contains file names
                .Where(f => extensions.Contains(System.IO.Path.GetExtension(f).ToLower())).ToArray(); //Puts File names in an array

            //int FolderCount = Directory.GetFiles(Location).Length; 
            int FolderCount = filepaths2.Length;//Gets size of the amount of image files in the directory
            z--;
            
            if (z == -1)   //Checks to see if the control variable is -1
            {
                z = FolderCount - 1; //Sets the control variable to the maximum amount of image files
            }

            currentimagenum--;

            if (currentimagenum == -1 || currentimagenum == 0)
            {
                currentimagenum = FolderCount; //sets currentnum = to the maximum amount of files IF its 0 or -1
            }

            //Throws message if no images are in the folder
            if (filepaths2.Length == 0)
            {
                System.Windows.MessageBox.Show("There are no Image files at the selected Location!!");
                return;
            }

            Point = filepaths2[z]; //Stores the current image path inside of the Point variable
            ImageName = System.IO.Path.GetFileName(Point);
            var uriSource = new Uri(Point, UriKind.Absolute); //uses the point variable to Load the current image
            PicHolder.Source = new BitmapImage(uriSource);
            ImgName.Text = ImageName;

            CurrentImageText.Text = Convert.ToString(currentimagenum); 
            MaxImagesText.Text = Convert.ToString(FolderCount);
        }
        #endregion

        #region NextButton
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            
            
            if (Location == null)
            {
                System.Windows.MessageBox.Show("You must Enter a Location!");
                return;
            }

            string[] extensions = { ".jpg", ".png", ".jpeg", ".gif" };

            string[] filepaths2 = Directory.GetFiles(Location, "*.*")
                .Where(f => extensions.Contains(System.IO.Path.GetExtension(f).ToLower())).ToArray(); //Puts File names in an arrau

            //int FolderCount = Directory.GetFiles(Location).Length; //Gets size of the folder images
            int FolderCount = filepaths2.Length;

            if (z >= FolderCount) //Checks to see if the current element is larger than the FolderCount size
                                  //Resets the counter to the beginning
            {
                z = 0;
            }

            z++;
            currentimagenum++;
            if (currentimagenum > FolderCount)
            {
                currentimagenum = 1;
            }

            if (z >= FolderCount) //Makes another check after the first one
            {
                z = 0;
            }

            //Throws message if no images are in the folder
            if(filepaths2.Length == 0)
            {
                System.Windows.MessageBox.Show("There are no Image files at the selected Location!!");
                return;
            }


            Point = filepaths2[z]; //Sets Where we are in the Array
            ImageName = System.IO.Path.GetFileName(Point);

            //PicHolder.Source = new BitmapImage(new Uri(@"/ImageGallery;component/PutImages/Test.png"));
            var uriSource = new Uri(Point, UriKind.Absolute);
            PicHolder.Source = new BitmapImage(uriSource);
            ImgName.Text = ImageName; //Changes the file name based on the Point Variable

            
            MaxImagesText.Text = Convert.ToString(FolderCount);
            CurrentImageText.Text = Convert.ToString(currentimagenum);
        }

        #endregion
        #region LocationButton
        private void Choose_Location_Click(object sender, RoutedEventArgs e)
        {
            
            FolderBrowserDialog FBD = new FolderBrowserDialog();
               
            if (FBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (Location == FBD.SelectedPath)
                {
                    System.Windows.MessageBox.Show("You've already selected this Directory");
                    return;
                }

                System.Windows.MessageBox.Show(FBD.SelectedPath);

                
                Location = FBD.SelectedPath;
                currentimagenum = 0;
            }

            LocationText.Text = Location;

            
        }
        #endregion

        #region Favorite Button
        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {

            FolderBrowserDialog FBD = new FolderBrowserDialog();

            if (FBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (Location == FBD.SelectedPath)
                {
                    System.Windows.MessageBox.Show("You've already selected this Directory");
                    return;
                }

                System.Windows.MessageBox.Show(FBD.SelectedPath);


                SaveLocation = FBD.SelectedPath;

                string destFile = System.IO.Path.Combine(SaveLocation, ImageName);

                System.IO.File.Copy(Point, destFile);
            }
        }
        #endregion
    }
}
