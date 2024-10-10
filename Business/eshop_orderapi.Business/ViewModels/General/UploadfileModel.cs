using System;

namespace IfourTechnolab.Business.ViewModels.General
{
    public class UploadfileModel
    {
        public int Id { get; set; }

        public string Filename { get; set; }

        public int? PostId { get; set; }

        public string Alt { get; set; }

        public string Title { get; set; }

        public Guid FileGuId { get; set; }
    }
}