using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Models;

namespace TenmoClient.DAL
{
    public class TransferApiDAO : ApiDAO, ITransferApiDAO
    {
        public TransferApiDAO(string apiUrl) : base(apiUrl) { }

        public IList<Transfer> ViewTransfers()
        {
            RestRequest request = new RestRequest($"transfers");
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);
            CheckResponse(response);
            List<Transfer> transfers = response.Data;
            return transfers;

        }
        public IList<Transfer> ViewRequests()
        {
            RestRequest request = new RestRequest($"transfers/requests");
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);
            CheckResponse(response);
            List<Transfer> transfers = response.Data;
            return transfers;

        }

        public Transfer SendTransfer(SendTransfer transfer)
        {
            RestRequest postRequest = new RestRequest("transfers");
            postRequest.AddJsonBody(transfer);
            IRestResponse<Transfer> response = client.Post<Transfer>(postRequest);
            CheckResponse(response);
           
            return response.Data;

        }

        public Transfer TransferDetails(int transferId)
        {
            RestRequest request = new RestRequest($"transfers/{transferId}");
            IRestResponse<Transfer> response = client.Get<Transfer>(request);
            CheckResponse(response);

            return response.Data;

        }

        public Transfer RequestTransfer(Transfer request)
        {
            RestRequest postRequest = new RestRequest("transfers/requests");
            postRequest.AddJsonBody(request);
            IRestResponse<Transfer> response = client.Post<Transfer>(postRequest);
            CheckResponse(response);

            return response.Data;

        }
    }
}