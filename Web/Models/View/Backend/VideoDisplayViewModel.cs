using System;
using System.Xml.Linq;
using MOROPO.Framework.Guama.Media.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View
{
    public class VideoDisplayViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityDisplayModel<Video, System.Guid>
    {
        public Guid Id { get; set; }

        public string Extension { get; set; }

        public long Length { get; set; }

        public TimeSpan Duration { get; set; }

        public string Format { get; set; }

        public string FrameSize { get; set; }

        public void Import(Video entity)
        {
            Id = entity.Id;
            Extension = entity.Extension;
            Length = entity.Length;
            Duration = entity.Duration;
            Format = entity.Format;
            FrameSize = entity.FrameSize;
        }
    }
}

