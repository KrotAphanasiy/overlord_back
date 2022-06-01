using AutoMapper;
using Flash.Central.Foundation.Base.Entities;
using Flash.Central.Foundation.Base.Models;
using Flash.Central.ViewModel.Camera;
using Flash.Central.ViewModel.DetectionEvent;
using Flash.Central.ViewModel.GasStation;
using Flash.Central.ViewModel.RecognitionEvent;
using Flash.Central.ViewModel.Visit;
using Flash.Domain.Entities;

namespace Flash.Central.Core.Configuration
{
    /// <summary>
    /// Class. Determines AutoMapper profile
    /// </summary>
    public class ModelToEntityMappingProfile : Profile
    {
        /// <summary>
        /// Creates mapping Entities classes and Model classes
        /// </summary>
        public ModelToEntityMappingProfile ()
        {
            CreateMap<BaseModel, BaseEntity>()
                .ForMember(x => x.IsDeleted, _ => _.Ignore())
                .ForMember(x => x.CreatedDate, _ => _.Ignore())
                .ForMember(x => x.UpdatedDate, _ => _.Ignore());

            CreateMap<BaseGuidModel, BaseGuidEntity>()
                .IncludeBase<BaseModel, BaseEntity>()
                .ForMember(x => x.Id, _ => _.Ignore());

            CreateMap<BaseLongModel, BaseLongEntity>()
                .IncludeBase<BaseModel, BaseEntity>()
                .ForMember(x => x.Id, _ => _.Ignore());


            CreateMap<CameraModel, Camera>()
                .IncludeBase<BaseGuidModel, BaseGuidEntity>()
                .ForMember(x => x.GasStation, _ => _.Ignore())
                .ForMember(x => x.Regions, _ => _.Ignore());

            CreateMap<CameraRegionModel, CameraRegion>()
                .IncludeBase<BaseLongModel, BaseLongEntity>()
                .ForMember(x => x.Camera, _ => _.Ignore())
                .ForMember(x => x.WatchingTerminal, _ => _.Ignore());

            CreateMap<RecognitionEventModel, RecognitionEvent>()
                .IncludeBase<BaseGuidModel, BaseGuidEntity>()
                .ForMember(x => x.VisitId, _ => _.Ignore())
                .ForMember(x => x.Visit, _ => _.Ignore())
                .ForMember(x => x.CameraRegion, _ => _.Ignore())
                .ForMember(x => x.ImageLink, _ => _.Ignore())
                .ForMember(x => x.IsIncorrectNumber, _ => _.Ignore())
                .ForMember(x => x.ProcessedImageLink, _ => _.Ignore());

            CreateMap<DetectionEventModel, DetectionEvent>()
                .IncludeBase<BaseGuidModel, BaseGuidEntity>()
                .ForMember(x => x.OriginalImageLink, _ => _.Ignore())
                .ForMember(x => x.CroppedImageLink, _ => _.Ignore())
                .ForMember(x => x.CameraRegion, _ => _.Ignore());


            CreateMap<VisitModel, Visit>()
                .IncludeBase<BaseLongModel, BaseLongEntity>()
                .ForMember(x=> x.GasStation, _ =>_.Ignore())
                .ForMember(x => x.RecognitionEvents, _ => _.Ignore());

            CreateMap<TerminalModel, Terminal>()
                .IncludeBase<BaseLongModel, BaseLongEntity>()
                .ForMember(x => x.AssignedCameraRegion, _ => _.Ignore())
                .ForMember(x => x.GasStation, _ => _.Ignore());

            CreateMap<GasStationModel, GasStation>()
                .IncludeBase<BaseLongModel, BaseLongEntity>()
                .ForMember(x => x.Cameras, _ => _.Ignore())
                .ForMember(x => x.Terminals, _ => _.Ignore());
        }
    }
}
