using DTOs.Response;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.DTOs.Request.Post;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Mappers
{
    public static class MediaContentMapper
    {
        public static MediaContentResponseDTO MediaContentToMediaContentResponseDTO(this MediaContent mediaContent)
        {
            if (mediaContent == null)
                throw new ArgumentNullException(nameof(mediaContent), "Media content cannot be null");

            var post = mediaContent.Post != null ? mediaContent.Post.PostToPostResponseDTO()
                                                  : new PostResponseDTO { Id = mediaContent.PostId };
            return new MediaContentResponseDTO
            {
                Id = mediaContent.Id,
                Media_type = mediaContent.Media_type,
                Media_Url = mediaContent.Media_Url,
                Post = post
            };
        }

        public static MediaContent MediaContentCreateDTOToMediaContent(this MediaContentCreateDTO mediaContentCreateDTO)
        {
            if (mediaContentCreateDTO == null)
                throw new ArgumentNullException(nameof(mediaContentCreateDTO), "MediaContentCreateDTO cannot be null");

            return new MediaContent
            {
                Media_type = mediaContentCreateDTO.Media_type,
                PostId = mediaContentCreateDTO.PostId
            };
        }

        public static MediaContent MediaContentUpdateDTOToMediaContent(this MediaContentUpdateDTO mediaContentUpdateDTO, MediaContent mediaContent)
        {
            if (mediaContentUpdateDTO == null)
                throw new ArgumentNullException(nameof(mediaContentUpdateDTO), "MediaContentUpdateDTO cannot be null");

            mediaContent.Media_type = mediaContentUpdateDTO.Media_type;
            mediaContent.PostId = mediaContentUpdateDTO.PostId;

            return mediaContent;
        }

        public static MediaContent MediaContentPatchDTOToMediaContent(this MediaContentPatchDTO mediaContentPatchDTO, MediaContent mediaContent)
        {
            if (mediaContentPatchDTO == null)
                throw new ArgumentNullException(nameof(mediaContentPatchDTO), "MediaContentPatchDTO cannot be null");

            if (mediaContentPatchDTO.Media_type != null)
                mediaContent.Media_type = mediaContentPatchDTO.Media_type;

            if (mediaContentPatchDTO.PostId != null)
                mediaContent.PostId = mediaContentPatchDTO.PostId.Value;

            return mediaContent;
        }
    }
}
