using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

namespace MiniAPI {

	public class API {

		private string baseURL = "";
		private Hashtable headers = new Hashtable();
		private Hashtable routes = new Hashtable();

		public delegate void GETCallback(HTTP.Request request);
		public delegate void GETListCallback(List<object> resObject);
		public delegate void GETDictCallback(Dictionary<string, object> resObject);

		/*
		 * Sets and stores auth token. If token expires then it must be reset with this function.
		 * Same result can be achieved with addHeader.
		 * @param string token
		 */
		public void SetAuthenticationHeader(string token){
			this.headers.Add("Authorization", "Bearer " + token);
		}

		public void AddHeader (string key, string value) {
			this.headers.Add(key, value);
		} 

		/**
		 * Sets Base API URL to be prepended to all api calls. If none it will be empty
		 * @param string baseURL
		 */
		public void SetBaseURL(string baseURL){
			this.baseURL = baseURL;
		}

		/**
		 * Adds an API Route to the record
		 * Use {0} variables to append paramenters later
		 */
		public void AddRoute (string key, string value){
			routes.Add(key, value);
		}

		/**
		 * Dels an API Route from the record
		 */ 
		public void DelRoute (string key) {
			routes.Remove(key);
		}

		/**
		 * Returns a Route value
		 */
		public string GetRoute (string key) {
			return routes[key] as string;
		}

		/**
		 * GET with plain request as callback
		 */
		public void GET (string endPoint, GETCallback cb) {
			HTTP.Request req = new HTTP.Request( "get", this.baseURL + endPoint );

			// Add headers
			foreach (DictionaryEntry h in headers) {
				req.AddHeader((string) h.Key, (string) h.Value);
			}

			req.Send( ( request ) => { cb(request); });
		}

		/**
		 * GET with result parsed as list
		 */
		public void GET (string endPoint, GETListCallback cb) {
			HTTP.Request req = new HTTP.Request( "get", this.baseURL + endPoint );
			
			// Add headers
			foreach (DictionaryEntry h in headers) {
				req.AddHeader((string) h.Key, (string) h.Value);
			}
			
			req.Send( ( request ) => { 
				var resList = Json.Deserialize(request.response.Text) as List<object>;
				cb(resList); 
			});
		}

		/**
		 * GET with result parsed as object
		 */
		public void GET (string endPoint, GETDictCallback cb) {
			HTTP.Request req = new HTTP.Request( "get", this.baseURL + endPoint );
			
			// Add headers
			foreach (DictionaryEntry h in headers) {
				req.AddHeader((string) h.Key, (string) h.Value);
			}
			
			req.Send( ( request ) => { 
				var resObj = Json.Deserialize(request.response.Text) as Dictionary<string, object>;
				cb(resObj); 
			});
		}


		/**
		 * POST with result parsed as Dictionary<string,object>
		 */
		public void POST (string endPoint, Hashtable body, GETDictCallback cb) {
			Console.WriteLine(this.baseURL + endPoint);
			HTTP.Request req = new HTTP.Request( "post", this.baseURL + endPoint, body );
			
			// Add headers
			foreach (DictionaryEntry h in headers) {
				req.AddHeader((string) h.Key, (string) h.Value);
			}
			
			req.Send( ( request ) => { 
				var resObj = Json.Deserialize(request.response.Text) as Dictionary<string, object>;
				cb(resObj); 
			});
		}

	}

}