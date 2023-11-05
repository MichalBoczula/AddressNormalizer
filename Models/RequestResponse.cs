namespace AddressNormalizer.Models
{
    public class RequestResponse
    {
        public string warning { get; set; }
        public string id { get; set; }
        public string model { get; set; }
        public string choices { get; set; }
        public string usage { get; set; }


        //{
        //    "warning": "This model version is deprecated. Migrate before January 4, 2024 to avoid disruption of service. Learn more https://platform.openai.com/docs/deprecations",
        //  "id": "cmpl-8HaIOxcN2mEudajp4UTe13ryY8PaO",
        //  "object": "text_completion",
        //  "created": 1699201788,
        //  "model": "text-davinci-003",
        //  "choices": [
        //    {
        //        "text": "\n\n{\n    \"city\": \"Wrocław\", \n    \"postalCode\": \"53-800\",\n    \"street\": \"Legnicka\",\n    \"buildingNumber\": \"33c/1\",\n    \"flatNumber\": null \n}",
        //      "index": 0,
        //      "logprobs": null,
        //      "finish_reason": "stop"
        //    }
        //  ],
        //  "usage": {
        //        "prompt_tokens": 40,
        //    "completion_tokens": 59,
        //    "total_tokens": 99
        //  }
        //}
    }
}
