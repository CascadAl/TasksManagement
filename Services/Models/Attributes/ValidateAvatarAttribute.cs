using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Services.Models.Attributes
{
    public class ValidateImageAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int maxContent = 480 * 480; //230 KB
            string[] allowedExt = { ".jpg", ".jpeg", ".gif", ".png" };

            var image = value as HttpPostedFileBase;

            if (image == null)
                return false;

            else if (!allowedExt.Contains(Path.GetExtension(image.FileName)))
            {
                ErrorMessage = "Please upload Your Photo of type: " + string.Join(", ", allowedExt);
                return false;
            }
            else if (image.ContentLength > maxContent)
            {
                ErrorMessage = "Your Photo is too large, maximum allowed size is : " + (maxContent / 1024).ToString() + "KB";
                return false;
            }
            else
                return true;
        }
    }
}
