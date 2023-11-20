using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace FoodShop_SWP.Models
{
    public class Summernote
    {
        public Summernote(string? iDEditor, bool loadLibrary = true)
        {
            IDEditor = iDEditor;
            LoadLibrary = loadLibrary;
        }

        public string? IDEditor { get; set; }
        public bool LoadLibrary { get; set; }
        public int height { get; set; } = 120;
        public string toolbar { get; set; } = @"
        
                    ['style', ['style']],
                    ['font', ['bold', 'underline', 'clear']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'paragraph']],
                    ['table', ['table']],
                    ['height', ['height']],
                    ['insert', ['link', 'picture', 'video']],
                    ['view', ['fullscreen', 'codeview', 'help']]
        ";

    }
}
