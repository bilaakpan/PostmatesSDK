namespace PostmatesSDK.Models
{
    /// <summary>
    /// 
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// 
        /// </summary>
        ResponseKinds Kind { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetResponseObject<T>() where T : BaseResponseModel;
    }
}