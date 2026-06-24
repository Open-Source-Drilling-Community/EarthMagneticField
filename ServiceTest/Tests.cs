using System.Net.Http.Headers;
using NORCE.Drilling.EarthMagneticField.ModelShared;

namespace ServiceTest
{
    public class Tests
    {
        // testing outside Visual Studio requires using http port (https faces authentication issues both in console and on github)
        private static string host = "http://localhost:8080/";
        //private static string host = "https://localhost:5001/";
        //private static string host = "https://localhost:44368/";
        //private static string host = "http://localhost:54949/";
        private static HttpClient httpClient;
        private static Client nSwagClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }; // temporary workaround for testing purposes: bypass certificate validation (not recommended for production environments due to security risks)
            httpClient = new HttpClient(handler);
            httpClient.BaseAddress = new Uri(host + "EarthMagneticField/api/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            nSwagClient = new Client(httpClient.BaseAddress.ToString(), httpClient);
        }

        [Test]
        public async Task Test_EarthMagneticFieldCalculationOrder_GET()
        {
            #region post a EarthMagneticFieldCalculationOrder
            // Create instance of EarthMagneticFieldCalculationOrder
            EarthMagneticFieldCalculationOrder earthMagneticFieldCalculationOrder = PseudoConstructors.ConstructEarthMagneticFieldCalculationOrder();

            //Extract metainfo
            MetaInfo metaInfo = earthMagneticFieldCalculationOrder.MetaInfo;
            Guid guid = metaInfo.ID;
            try
            {
                await nSwagClient.PostEarthMagneticFieldCalculationOrderAsync(earthMagneticFieldCalculationOrder);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to POST given EarthMagneticFieldCalculationOrder\n" + ex.Message);
            }
            #endregion

            #region GetAllEarthMagneticFieldCalculationOrderId
            List<Guid> idList = [];
            try
            {
                idList = (List<Guid>)await nSwagClient.GetAllEarthMagneticFieldCalculationOrderIdAsync();
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET all EarthMagneticFieldCalculationOrder ids\n" + ex.Message);
            }
            Assert.That(idList, Is.Not.Null);
            Assert.That(idList, Does.Contain(guid));
            #endregion

            #region GetAllEarthMagneticFieldCalculationOrderMetaInfo
            List<MetaInfo> metaInfoList = [];
            try
            {
                metaInfoList = (List<MetaInfo>)await nSwagClient.GetAllEarthMagneticFieldCalculationOrderMetaInfoAsync();
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET all EarthMagneticFieldCalculationOrder metainfos\n" + ex.Message);
            }
            Assert.That(metaInfoList, Is.Not.Null);
            IEnumerable<MetaInfo> metaInfoList2 =
                from elt in metaInfoList
                where elt.ID == guid
                select elt;
            Assert.That(metaInfoList2, Is.Not.Null);
            Assert.That(metaInfoList2, Is.Not.Empty);
            #endregion

            #region GetAllEarthMagneticFieldCalculationOrderById
            EarthMagneticFieldCalculationOrder? earthMagneticFieldCalculationOrder2 = null;
            try
            {
                earthMagneticFieldCalculationOrder2 = await nSwagClient.GetEarthMagneticFieldCalculationOrderByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET EarthMagneticFieldCalculationOrder of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticFieldCalculationOrder2, Is.Not.Null);
            Assert.That(earthMagneticFieldCalculationOrder2.Name, Is.EqualTo(earthMagneticFieldCalculationOrder.Name));
            #endregion

            #region GetAllEarthMagneticFieldCalculationOrderLight
            List<EarthMagneticFieldCalculationOrderLight> earthMagneticFieldCalculationOrderLightList = [];
            try
            {
                earthMagneticFieldCalculationOrderLightList = (List<EarthMagneticFieldCalculationOrderLight>)await nSwagClient.GetAllEarthMagneticFieldCalculationOrderLightAsync();
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET the list of EarthMagneticFieldCalculationOrderLight\n" + ex.Message);
            }
            Assert.That(earthMagneticFieldCalculationOrderLightList, Is.Not.Null);
            Assert.That(earthMagneticFieldCalculationOrderLightList, Is.Not.Empty);
            IEnumerable<EarthMagneticFieldCalculationOrderLight> earthMagneticFieldCalculationOrderLightList2 =
                from elt in earthMagneticFieldCalculationOrderLightList
                where elt.Name == earthMagneticFieldCalculationOrder.Name
                select elt;
            Assert.That(earthMagneticFieldCalculationOrderLightList2, Is.Not.Null);
            Assert.That(earthMagneticFieldCalculationOrderLightList2, Is.Not.Empty);
            #endregion

            #region GetAllEarthMagneticFieldCalculationOrder
            List<EarthMagneticFieldCalculationOrder> earthMagneticFieldCalculationOrderList = new();
            try
            {
                earthMagneticFieldCalculationOrderList = (List<EarthMagneticFieldCalculationOrder>)await nSwagClient.GetAllEarthMagneticFieldCalculationOrderAsync();
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET the list of EarthMagneticFieldCalculationOrder\n" + ex.Message);
            }
            Assert.That(earthMagneticFieldCalculationOrderList, Is.Not.Null);
            IEnumerable<EarthMagneticFieldCalculationOrder> earthMagneticFieldCalculationOrderList2 =
                from elt in earthMagneticFieldCalculationOrderList
                where elt.Name == earthMagneticFieldCalculationOrder.Name
                select elt;
            Assert.That(earthMagneticFieldCalculationOrderList2, Is.Not.Null);
            Assert.That(earthMagneticFieldCalculationOrderList2, Is.Not.Empty);
            #endregion

            #region finally delete the new ID
            earthMagneticFieldCalculationOrder2 = null;
            try
            {
                await nSwagClient.DeleteEarthMagneticFieldCalculationOrderByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to DELETE EarthMagneticFieldCalculationOrder of given Id\n" + ex.Message);
            }
            try
            {
                earthMagneticFieldCalculationOrder2 = await nSwagClient.GetEarthMagneticFieldCalculationOrderByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(404));
                TestContext.WriteLine("Impossible to GET EarthMagneticFieldCalculationOrder of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticFieldCalculationOrder2, Is.Null);
            #endregion
        }

        [Test]
        public async Task Test_EarthMagneticFieldCalculationOrder_POST()
        {
            #region trying to post an empty guid
            // Create instance of earthMagneticFieldCalculationOrder
            EarthMagneticFieldCalculationOrder earthMagneticFieldCalculationOrder = PseudoConstructors.ConstructEarthMagneticFieldCalculationOrder();
            earthMagneticFieldCalculationOrder.MetaInfo.ID = Guid.Empty;
            //Extract metainfo
            MetaInfo metaInfo = earthMagneticFieldCalculationOrder.MetaInfo;
            EarthMagneticFieldCalculationOrder? earthMagneticFieldCalculationOrder2 = null;
            try
            {
                await nSwagClient.PostEarthMagneticFieldCalculationOrderAsync(earthMagneticFieldCalculationOrder);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(400));
                TestContext.WriteLine("Impossible to POST EarthMagneticFieldCalculationOrder with empty Guid\n" + ex.Message);
            }
            try
            {
                earthMagneticFieldCalculationOrder2 = await nSwagClient.GetEarthMagneticFieldCalculationOrderByIdAsync(Guid.Empty);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(400));
                TestContext.WriteLine("Impossible to GET EarthMagneticFieldCalculationOrder identified by an empty Guid\n" + ex.Message);
            }
            Assert.That(earthMagneticFieldCalculationOrder2, Is.Null);
            #endregion

            #region post some corrupted data
            // post data with missing input that fails the calculation process
            #endregion

            #region posting a new ID in a valid state
            Guid guid = Guid.NewGuid();
            metaInfo = new() { ID = guid };
            earthMagneticFieldCalculationOrder.MetaInfo = metaInfo;
            try
            {
                await nSwagClient.PostEarthMagneticFieldCalculationOrderAsync(earthMagneticFieldCalculationOrder);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to POST EarthMagneticFieldCalculationOrder although it is in a valid state\n" + ex.Message);
            }
            try
            {
                earthMagneticFieldCalculationOrder2 = await nSwagClient.GetEarthMagneticFieldCalculationOrderByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET the EarthMagneticFieldCalculationOrder of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticFieldCalculationOrder2, Is.Not.Null);
            Assert.That(earthMagneticFieldCalculationOrder2.MetaInfo, Is.Not.Null);
            Assert.That(earthMagneticFieldCalculationOrder2.MetaInfo.ID, Is.EqualTo(guid));
            Assert.That(earthMagneticFieldCalculationOrder2.Name, Is.EqualTo(earthMagneticFieldCalculationOrder.Name));
            #endregion

            #region trying to repost the same ID
            bool conflict = false;
            try
            {
                await nSwagClient.PostEarthMagneticFieldCalculationOrderAsync(earthMagneticFieldCalculationOrder);
            }
            catch (ApiException ex)
            {
                conflict = true;
                Assert.That(ex.StatusCode, Is.EqualTo(409));
                TestContext.WriteLine("Impossible to POST existing EarthMagneticFieldCalculationOrder\n" + ex.Message);
            }
            Assert.That(conflict, Is.True);
            #endregion

            #region finally delete the new ID
            earthMagneticFieldCalculationOrder2 = null;
            try
            {
                await nSwagClient.DeleteEarthMagneticFieldCalculationOrderByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to DELETE EarthMagneticFieldCalculationOrder of given Id\n" + ex.Message);
            }
            try
            {
                earthMagneticFieldCalculationOrder2 = await nSwagClient.GetEarthMagneticFieldCalculationOrderByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(404));
                TestContext.WriteLine("Impossible to GET deleted EarthMagneticFieldCalculationOrder of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticFieldCalculationOrder2, Is.Null);
            #endregion
        }

        [Test]
        public async Task Test_EarthMagneticFieldCalculationOrder_PUT()
        {
            #region posting a new ID
            // Create instance of earthMagneticFieldCalculationOrder
            EarthMagneticFieldCalculationOrder earthMagneticFieldCalculationOrder = PseudoConstructors.ConstructEarthMagneticFieldCalculationOrder();
            //Extract metainfo
            MetaInfo metaInfo = earthMagneticFieldCalculationOrder.MetaInfo;
            Guid guid = metaInfo.ID;
            EarthMagneticFieldCalculationOrder? earthMagneticFieldCalculationOrder2 = null;
            try
            {
                await nSwagClient.PostEarthMagneticFieldCalculationOrderAsync(earthMagneticFieldCalculationOrder);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to POST EarthMagneticFieldCalculationOrder\n" + ex.Message);
            }
            try
            {
                earthMagneticFieldCalculationOrder2 = await nSwagClient.GetEarthMagneticFieldCalculationOrderByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET the EarthMagneticFieldCalculationOrder of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticFieldCalculationOrder2, Is.Not.Null);
            Assert.That(earthMagneticFieldCalculationOrder2.MetaInfo, Is.Not.Null);
            Assert.That(earthMagneticFieldCalculationOrder2.MetaInfo.ID, Is.EqualTo(guid));
            Assert.That(earthMagneticFieldCalculationOrder2.Name, Is.EqualTo(earthMagneticFieldCalculationOrder.Name));
            #endregion

            #region updating the new Id
            earthMagneticFieldCalculationOrder.Name = "My test EarthMagneticFieldCalculationOrder with modified name";
            earthMagneticFieldCalculationOrder.LastModificationDate = DateTimeOffset.UtcNow;
            try
            {
                await nSwagClient.PutEarthMagneticFieldCalculationOrderByIdAsync(earthMagneticFieldCalculationOrder.MetaInfo.ID, earthMagneticFieldCalculationOrder);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to PUT EarthMagneticFieldCalculationOrder of given Id\n" + ex.Message);
            }
            try
            {
                earthMagneticFieldCalculationOrder2 = await nSwagClient.GetEarthMagneticFieldCalculationOrderByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET the updated EarthMagneticFieldCalculationOrder of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticFieldCalculationOrder2, Is.Not.Null);
            Assert.That(earthMagneticFieldCalculationOrder2.MetaInfo, Is.Not.Null);
            Assert.That(earthMagneticFieldCalculationOrder2.MetaInfo.ID, Is.EqualTo(earthMagneticFieldCalculationOrder.MetaInfo.ID));
            Assert.That(earthMagneticFieldCalculationOrder2.Name, Is.EqualTo(earthMagneticFieldCalculationOrder.Name));
            #endregion

            #region finally delete the new ID
            earthMagneticFieldCalculationOrder2 = null;
            try
            {
                await nSwagClient.DeleteEarthMagneticFieldCalculationOrderByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to DELETE EarthMagneticFieldCalculationOrder of given Id\n" + ex.Message);
            }
            try
            {
                earthMagneticFieldCalculationOrder2 = await nSwagClient.GetEarthMagneticFieldCalculationOrderByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(404));
                TestContext.WriteLine("Impossible to GET deleted EarthMagneticFieldCalculationOrder of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticFieldCalculationOrder2, Is.Null);
            #endregion
        }

        [Test]
        public async Task Test_EarthMagneticFieldCalculationOrder_DELETE()
        {
            #region posting a new ID
            // Create instance of earthMagneticFieldCalculationOrder
            EarthMagneticFieldCalculationOrder earthMagneticFieldCalculationOrder = PseudoConstructors.ConstructEarthMagneticFieldCalculationOrder();
            //Extract metainfo
            MetaInfo metaInfo = earthMagneticFieldCalculationOrder.MetaInfo;
            Guid guid = metaInfo.ID;
            EarthMagneticFieldCalculationOrder? earthMagneticFieldCalculationOrder2 = null;
            try
            {
                await nSwagClient.PostEarthMagneticFieldCalculationOrderAsync(earthMagneticFieldCalculationOrder);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to POST EarthMagneticFieldCalculationOrder\n" + ex.Message);
            }
            try
            {
                earthMagneticFieldCalculationOrder2 = await nSwagClient.GetEarthMagneticFieldCalculationOrderByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET the EarthMagneticFieldCalculationOrder of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticFieldCalculationOrder2, Is.Not.Null);
            Assert.That(earthMagneticFieldCalculationOrder2.MetaInfo, Is.Not.Null);
            Assert.That(earthMagneticFieldCalculationOrder2.MetaInfo.ID, Is.EqualTo(earthMagneticFieldCalculationOrder.MetaInfo.ID));
            Assert.That(earthMagneticFieldCalculationOrder2.Name, Is.EqualTo(earthMagneticFieldCalculationOrder.Name));
            #endregion

            #region finally delete the new ID
            earthMagneticFieldCalculationOrder2 = null;
            try
            {
                await nSwagClient.DeleteEarthMagneticFieldCalculationOrderByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to DELETE EarthMagneticFieldCalculationOrder of given Id\n" + ex.Message);
            }
            try
            {
                earthMagneticFieldCalculationOrder2 = await nSwagClient.GetEarthMagneticFieldCalculationOrderByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(404));
                TestContext.WriteLine("Impossible to GET deleted EarthMagneticFieldCalculationOrder of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticFieldCalculationOrder2, Is.Null);
            #endregion
        }

        [Test]
        public async Task Test_EarthMagneticField_GET()
        {
            #region post a EarthMagneticField
            // Create instance of earthMagneticField
            EarthMagneticField earthMagneticField = PseudoConstructors.ConstructEarthMagneticField();
            MetaInfo metaInfo = earthMagneticField.MetaInfo;
            Guid guid = metaInfo.ID;

            try
            {
                await nSwagClient.PostEarthMagneticFieldAsync(earthMagneticField);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to POST given EarthMagneticField\n" + ex.Message);
            }
            #endregion

            #region GetAllEarthMagneticFieldId
            List<Guid?> idList = [];
            try
            {
                idList = (List<Guid?>)await nSwagClient.GetAllEarthMagneticFieldIdAsync();
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET all EarthMagneticField ids\n" + ex.Message);
            }
            Assert.That(idList, Is.Not.Null);
            Assert.That(idList, Does.Contain(guid));
            #endregion

            #region GetAllEarthMagneticFieldMetaInfo
            List<MetaInfo> metaInfoList = [];
            try
            {
                metaInfoList = (List<MetaInfo>)await nSwagClient.GetAllEarthMagneticFieldMetaInfoAsync();
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET all EarthMagneticField metainfos\n" + ex.Message);
            }
            Assert.That(metaInfoList, Is.Not.Null);
            IEnumerable<MetaInfo> metaInfoList2 =
                from elt in metaInfoList
                where elt.ID == guid
                select elt;
            Assert.That(metaInfoList2, Is.Not.Null);
            Assert.That(metaInfoList2, Is.Not.Empty);
            #endregion

            #region GetAllEarthMagneticFieldById
            EarthMagneticField? earthMagneticField2 = null;
            try
            {
                earthMagneticField2 = await nSwagClient.GetEarthMagneticFieldByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET EarthMagneticField of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticField2, Is.Not.Null);
            Assert.That(earthMagneticField2.MetaInfo.ID, Is.EqualTo(guid));
            Assert.That(earthMagneticField2.Name, Is.EqualTo(earthMagneticField.Name));
            #endregion

            #region GetAllEarthMagneticField
            List<EarthMagneticField> earthMagneticFieldList = [];
            try
            {
                earthMagneticFieldList = (List<EarthMagneticField>)await nSwagClient.GetAllEarthMagneticFieldAsync();
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET the list of EarthMagneticField\n" + ex.Message);
            }
            Assert.That(earthMagneticFieldList, Is.Not.Null);
            IEnumerable<EarthMagneticField> earthMagneticFieldList2 =
                from elt in earthMagneticFieldList
                where elt.Name == earthMagneticField.Name
                select elt;
            Assert.That(earthMagneticFieldList2, Is.Not.Null);
            Assert.That(earthMagneticFieldList2, Is.Not.Empty);
            #endregion

            #region finally delete the new ID
            earthMagneticField2 = null;
            try
            {
                await nSwagClient.DeleteEarthMagneticFieldByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to DELETE EarthMagneticField of given Id\n" + ex.Message);
            }
            try
            {
                earthMagneticField2 = await nSwagClient.GetEarthMagneticFieldByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(404));
                TestContext.WriteLine("Impossible to GET EarthMagneticField of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticField2, Is.Null);
            #endregion
        }

        [Test]
        public async Task Test_EarthMagneticField_POST()
        {
            #region trying to post an empty guid
            // Create instance of earthMagneticField
            EarthMagneticField earthMagneticField = PseudoConstructors.ConstructEarthMagneticField();
            MetaInfo metaInfo = earthMagneticField.MetaInfo;

            EarthMagneticField? earthMagneticField2 = null;
            try
            {
                await nSwagClient.PostEarthMagneticFieldAsync(earthMagneticField);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(400));
                TestContext.WriteLine("Impossible to POST EarthMagneticField with empty Guid\n" + ex.Message);
            }
            try
            {
                earthMagneticField2 = await nSwagClient.GetEarthMagneticFieldByIdAsync(Guid.Empty);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(400));
                TestContext.WriteLine("Impossible to GET EarthMagneticField identified by an empty Guid\n" + ex.Message);
            }
            Assert.That(earthMagneticField2, Is.Null);
            #endregion

            #region posting a new ID in a valid state
            Guid guid = Guid.NewGuid();
            metaInfo = new() { ID = guid };
            earthMagneticField.MetaInfo = metaInfo;
            try
            {
                await nSwagClient.PostEarthMagneticFieldAsync(earthMagneticField);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to POST EarthMagneticField although it is in a valid state\n" + ex.Message);
            }
            try
            {
                earthMagneticField2 = await nSwagClient.GetEarthMagneticFieldByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET the EarthMagneticField of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticField2, Is.Not.Null);
            Assert.That(earthMagneticField2.MetaInfo, Is.Not.Null);
            Assert.That(earthMagneticField2.MetaInfo.ID, Is.EqualTo(guid));
            Assert.That(earthMagneticField2.Name, Is.EqualTo(earthMagneticField.Name));
            #endregion

            #region trying to repost the same ID
            bool conflict = false;
            try
            {
                await nSwagClient.PostEarthMagneticFieldAsync(earthMagneticField);
            }
            catch (ApiException ex)
            {
                conflict = true;
                Assert.That(ex.StatusCode, Is.EqualTo(409));
                TestContext.WriteLine("Impossible to POST existing EarthMagneticField\n" + ex.Message);
            }
            Assert.That(conflict, Is.True);
            #endregion

            #region finally delete the new ID
            earthMagneticField2 = null;
            try
            {
                await nSwagClient.DeleteEarthMagneticFieldByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to DELETE EarthMagneticField of given Id\n" + ex.Message);
            }
            try
            {
                earthMagneticField2 = await nSwagClient.GetEarthMagneticFieldByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(404));
                TestContext.WriteLine("Impossible to GET deleted EarthMagneticField of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticField2, Is.Null);
            #endregion
        }

        [Test]
        public async Task Test_EarthMagneticField_PUT()
        {
            #region posting a new ID
            // Create instance of earthMagneticField
            EarthMagneticField earthMagneticField = PseudoConstructors.ConstructEarthMagneticField();
            MetaInfo metaInfo = earthMagneticField.MetaInfo;
            Guid guid = metaInfo.ID;

            EarthMagneticField? earthMagneticField2 = null;
            try
            {
                await nSwagClient.PostEarthMagneticFieldAsync(earthMagneticField);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to POST EarthMagneticField\n" + ex.Message);
            }
            try
            {
                earthMagneticField2 = await nSwagClient.GetEarthMagneticFieldByIdAsync(earthMagneticField.MetaInfo.ID);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET the EarthMagneticField of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticField2, Is.Not.Null);
            Assert.That(earthMagneticField2.MetaInfo, Is.Not.Null);
            Assert.That(earthMagneticField2.MetaInfo.ID, Is.EqualTo(earthMagneticField.MetaInfo.ID));
            Assert.That(earthMagneticField2.Name, Is.EqualTo(earthMagneticField.Name));
            #endregion

            #region updating the new Id
            earthMagneticField.Name = "My test EarthMagneticField with modified name";
            earthMagneticField.LastModificationDate = DateTimeOffset.UtcNow;
            try
            {
                await nSwagClient.PutEarthMagneticFieldByIdAsync(earthMagneticField.MetaInfo.ID, earthMagneticField);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to PUT EarthMagneticField of given Id\n" + ex.Message);
            }
            try
            {
                earthMagneticField2 = await nSwagClient.GetEarthMagneticFieldByIdAsync(earthMagneticField.MetaInfo.ID);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET the updated EarthMagneticField of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticField2, Is.Not.Null);
            Assert.That(earthMagneticField2.MetaInfo, Is.Not.Null);
            Assert.That(earthMagneticField2.MetaInfo.ID, Is.EqualTo(earthMagneticField.MetaInfo.ID));
            Assert.That(earthMagneticField2.Name, Is.EqualTo(earthMagneticField.Name));
            #endregion

            #region finally delete the new ID
            earthMagneticField2 = null;
            try
            {
                await nSwagClient.DeleteEarthMagneticFieldByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to DELETE EarthMagneticField of given Id\n" + ex.Message);
            }
            try
            {
                earthMagneticField2 = await nSwagClient.GetEarthMagneticFieldByIdAsync(earthMagneticField.MetaInfo.ID);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(404));
                TestContext.WriteLine("Impossible to GET deleted EarthMagneticField of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticField2, Is.Null);
            #endregion
        }

        [Test]
        public async Task Test_EarthMagneticField_DELETE()
        {
            #region posting a new ID
            // Create instance of earthMagneticField
            EarthMagneticField earthMagneticField = PseudoConstructors.ConstructEarthMagneticField();
            MetaInfo metaInfo = earthMagneticField.MetaInfo;
            Guid guid = metaInfo.ID;

            EarthMagneticField? earthMagneticField2 = null;
            try
            {
                await nSwagClient.PostEarthMagneticFieldAsync(earthMagneticField);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to POST EarthMagneticField\n" + ex.Message);
            }
            try
            {
                earthMagneticField2 = await nSwagClient.GetEarthMagneticFieldByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET the EarthMagneticField of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticField2, Is.Not.Null);
            Assert.That(earthMagneticField2.MetaInfo, Is.Not.Null);
            Assert.That(earthMagneticField2.MetaInfo.ID, Is.EqualTo(guid));
            Assert.That(earthMagneticField2.Name, Is.EqualTo(earthMagneticField.Name));
            #endregion

            #region finally delete the new ID
            earthMagneticField2 = null;
            try
            {
                await nSwagClient.DeleteEarthMagneticFieldByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to DELETE EarthMagneticField of given Id\n" + ex.Message);
            }
            try
            {
                earthMagneticField2 = await nSwagClient.GetEarthMagneticFieldByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(404));
                TestContext.WriteLine("Impossible to GET deleted EarthMagneticField of given Id\n" + ex.Message);
            }
            Assert.That(earthMagneticField2, Is.Null);
            #endregion
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            httpClient?.Dispose();
        }
    }
}