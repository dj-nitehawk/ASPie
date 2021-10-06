﻿using FastEndpoints;

namespace Uploads.Image.Save
{
    public class Endpoint : Endpoint<Request>
    {
        public Endpoint()
        {
            Verbs(Http.POST);
            Routes("uploads/image/save");
            AllowFileUploads();
            AllowAnnonymous();
        }

        protected override Task HandleAsync(Request r, CancellationToken ct)
        {
            if (Files.Count > 0)
            {
                var file = Files[0];
                return SendStreamAsync(file.OpenReadStream(), null, file.Length, "image/jpeg", ct);
            }

            return SendNoContentAsync();
        }
    }
}