using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlickrUploader
{
	public class Album : List<Photo>
	{
		public string SetId { get; set; }
		private string FirstPhotoId { get; set; }

		public string FindFirstPhotoId()
		{
			if(!string.IsNullOrWhiteSpace(FirstPhotoId))
			{
				return FirstPhotoId;
			}

			foreach(Photo p in this)
			{
				if(!string.IsNullOrWhiteSpace(p.PhotoId))
				{
					FirstPhotoId = p.PhotoId;
					return p.PhotoId;
				}
			}
			return null;
		}

		public Photo UploadFailedPhotoInfo()
		{
			foreach(Photo p in this)
			{
				if(string.IsNullOrWhiteSpace(p.PhotoId))
				{
					return p;
				}
			}
			return null;
		}

		public bool IsNotTheFirstPhoto(string photoId)
		{
			return !this.FirstPhotoId.Equals(photoId);
		}
	}
}
