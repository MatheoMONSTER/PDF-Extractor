using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PDFEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf";

            if(openFileDialog.ShowDialog() == true)
            {
                FilePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void ExtractTextButton_Click(Object sender, RoutedEventArgs e)
        {
            string filePath = FilePathTextBox.Text;
            if(string.IsNullOrEmpty(filePath) )
            {
                MessageBox.Show("Please select a PDF file");
                return;
            }

            try
            {
                using (PdfReader reader = new PdfReader(filePath))
                {
                    string text = string.Empty;
                    for(int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        text += PdfTextExtractor.GetTextFromPage(reader, i); 
                    }
                    MessageBox.Show("Extracted text:\n\n" + text);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void AddWatermarkButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FilePathTextBox.Text;
            if(string.IsNullOrEmpty(filePath) )
            {
                MessageBox.Show("Please select a PDF file");
                return;
            }

            try
            {
                using (PdfReader reader = new PdfReader(filePath))
                using (PdfStamper stamper = new PdfStamper(reader, new System.IO.FileStream("output.pdf", System.IO.FileMode.Create)))
                {
                    int pageCount = reader.NumberOfPages;

                    // Add watermark to each page
                    for (int i = 1; i <= pageCount; i++)
                    {
                        PdfContentByte canvas = stamper.GetUnderContent(i);
                        Font font = new Font(Font.FontFamily.HELVETICA, 48, Font.BOLD, BaseColor.RED);
                        ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, new Phrase("Watermark", font), 300, 400, 45);
                    }

                    MessageBox.Show("Watermark added successfully. Output file: output.pdf");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
