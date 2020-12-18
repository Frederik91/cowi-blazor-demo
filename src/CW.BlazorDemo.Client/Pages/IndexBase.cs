using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CW.BlazorDemo.Client.Models;
using CW.Core;
using Microsoft.AspNetCore.Components;

namespace CW.BlazorDemo.Client.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject] private IDocumentController DocumentController { get; set; }
        [Inject] protected ISession Session { get; set; }

        [Parameter] public int Port { get; set; }

        public List<CW_Document> Links { get; set; } = new List<CW_Document>();
        public CW_Document Document { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Session.SelectedPort > 0)
                await UpdateDocuments();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (Port == 0)
                return;

            Session.SelectedPort = Port;
            if (!Session.AvailablePorts.Contains(Port))
                Session.AvailablePorts.Add(Port);

            await UpdateDocuments();
        }

        protected async Task UpdateDocuments()
        {
            var documents = await DocumentController.Get();
            Links = new List<CW_Document>(documents.Where(x => x.IsLinked));
            Document = documents.FirstOrDefault(x => !x.IsLinked);
        }
    }
}