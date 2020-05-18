using BackgroundApplication2.Model;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace BackgroundApplication2.Controller
{
    /// <summary>
    /// Sample controller with all verbs supporting a response content.
    /// </summary>
    [RestController(InstanceCreationType.PerCall)]
    public sealed class RestController
    {

        //Login

        [UriFormat("/device/getLogin")]
        public IGetResponse GetLogin()
        {

            return new GetResponse(
                GetResponse.ResponseStatus.OK, HardwareController.GetLogin(3));
        }

        [UriFormat("/device/setLogin")]
        public IPutResponse SetLogin([FromContent] LoginData login)
        {

            return new PutResponse(
                PutResponse.ResponseStatus.OK,
               HardwareController.SetLogin(login));
        }

        //Device

        [UriFormat("/device/getActive")]
        public IGetResponse GetActive()
        {
            
            return new GetResponse(
                GetResponse.ResponseStatus.OK, HardwareController.GetDeviceInfo());
        }

        [UriFormat("/device/setActive")]
        public IPutResponse SetActive([FromContent] Zustand zustand)
        {

            return new PutResponse(
                PutResponse.ResponseStatus.OK,
               HardwareController.SetActivate(zustand.Active));
        }

        [UriFormat("/device/getNetwork")]
        public IGetResponse GetHardwareInfo()
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                HardwareController.GetNetwork());
        }

        //Tabs

        [UriFormat("/tabs/getActivated")]
        public IGetResponse GetActiveTabs()
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                TabsController.GetTab());
        }

        [UriFormat("/tabs/setActivated")]
        public IPutResponse SetActiveTabs([FromContent] Tabs tabs)
        {
            return new PutResponse(
                PutResponse.ResponseStatus.OK,
                TabsController.SaveTab(tabs));
        }

        //Infoleiste

        [UriFormat("/infoleiste/getLeiste")]
        public IGetResponse GetInfoTitle()
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                LeistenController.GetProfil());
        }

        [UriFormat("/infoleiste/setLeiste")]
        public IPutResponse SetInfoTitle([FromContent] Infoleiste infoleiste)
        {
            return new PutResponse(
                PutResponse.ResponseStatus.OK,
                LeistenController.SaveProfil(infoleiste));
        }


        //Allgemein

        [UriFormat("/allgemein/getProfil")]
        public IGetResponse GetProfil()
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                ProfilController.GetProfil());
        }


        [UriFormat("/allgemein/setProfil")]
        public IPutResponse SetProfil([FromContent, ReadOnlyArray]Profil[] profil)
        {
            return new PutResponse(
                PutResponse.ResponseStatus.OK,
                ProfilController.SaveProfil(profil));
        }


        //Aktuelles

        [UriFormat("/aktuelles/getAktuellesText")]
        public IGetResponse GetAktuellesText()
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                AktuellesController.GetProfil());
        }

        [UriFormat("/aktuelles/setAktuellesText")]
        public IPutResponse SetAktuellesText([FromContent]Aktuelles aktuelles)
        {
            return new PutResponse(
                PutResponse.ResponseStatus.OK,
                AktuellesController.SaveProfil(aktuelles));
        
        }

 
        //Veranstaltung

        [UriFormat("/veranstaltung/getAktuell")]
        public IGetResponse GetAktuelleVeranstaltung()
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                VeranstaltungsController.GetText());
        }

        [UriFormat("/veranstaltung/setAktuell")]
        public IPutResponse SetAktuelleVeranstaltung([FromContent, ReadOnlyArray] Veranstaltung[] veranstaltung)
        {
            return new PutResponse(
                PutResponse.ResponseStatus.OK,
                VeranstaltungsController.SaveText(veranstaltung));
        }

        //Belegungsplan

        [UriFormat("/belegungsplan/getBelegungsplan")]
        public IGetResponse GetBelegungsplan()
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                BelegungsPlanController.GetAssignment());
        }

        [UriFormat("/belegungsplan/setBelegungsplan")]
        public IPutResponse SetBelegungsplan([FromContent] BelegungsPlan belegungsPlan)
        {
            return new PutResponse(
                PutResponse.ResponseStatus.OK,
                BelegungsPlanController.SaveAssignment(belegungsPlan));
        }


        //Wetter

        [UriFormat("/wetter/getOpenWeather")]
        public IGetResponse GetOpenWeather()
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                WetterController.GetAssignment());
        }

        [UriFormat("/wetter/setOpenWeather")]
        public IPutResponse SetOpenWeather([FromContent] OpenWeather weather)
        {
            return new PutResponse(
                PutResponse.ResponseStatus.OK,
                WetterController.SaveAssignment(weather));
        }



        //Prüfungs Application starten und beenden
        [UriFormat("/pruefung/activate")]
        public IPostResponse ActivatePruefungsapp([FromContent] Zustand zustand)
        {
            return new PostResponse(
                PostResponse.ResponseStatus.Created,
                HardwareController.ActivateExam(zustand.Active));
        }


        /*
        [UriFormat("/withresponsecontent")]
        public IPutResponse UpdateSomething()
        {
            return new PutResponse(
                PutResponse.ResponseStatus.OK,
                new ResponseData() { Status = "PUT received" });
        }

        [UriFormat("/withresponsecontent")]
        public IPostResponse CreateSomething()
        {
            return new PostResponse(
                PostResponse.ResponseStatus.Created,
                "newlocation",
                new ResponseData() { Status = "POST received" });
        }

        [UriFormat("/withresponsecontent")]
        public IGetResponse GetSomething()
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                new ResponseData() { Status = "GET received" });
        }

        [UriFormat("/withresponsecontent")]
        public IDeleteResponse DeleteSomething()
        {
            return new DeleteResponse(DeleteResponse.ResponseStatus.OK);
        }
        */
    }

}
