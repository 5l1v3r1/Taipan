// --------------------------------------------------------------------------------------
// Script that contains the official Taipan templates used in production
// --------------------------------------------------------------------------------------

// load the Debug version of the Assemblies
#r "packages/FSharpLog/lib/ES.Fslog.dll"
#r "System.Xml.Linq"

open System
open System.IO

#r "build/Taipan/ES.Taipan.Infrastructure.dll"
#r "build/Taipan/ES.Taipan.Crawler.dll"
#r "build/Taipan/ES.Taipan.Crawler.WebScrapers.dll"
#r "build/Taipan/ES.Taipan.Discoverer.dll"
#r "build/Taipan/ES.Taipan.Fingerprinter.dll"
#r "build/Taipan/ES.Taipan.Inspector.dll"
#r "build/Taipan/ES.Taipan.Application.dll"
#r "build/Taipan/ES.Taipan.Inspector.AddOns.dll"

open ES.Taipan.Application
open ES.Taipan.Crawler
open ES.Taipan.Crawler.WebScrapers
open ES.Taipan.Inspector.AddOns

let createTemplate(name: String, guid: String) =
    let defaultProfile = new TemplateProfile(Id = Guid.Parse(guid), Name = name)
    defaultProfile.RunCrawler <- false  
    defaultProfile.RunResourceDiscoverer <- false
    defaultProfile.RunWebAppFingerprinter <- false
    defaultProfile.RunVulnerabilityScanner <- false
    
    // inspector settings
    defaultProfile.VulnerabilityScannerSettings.ActivateAllAddOns <- true
        
    // discoverer settings
    defaultProfile.ResourceDiscovererSettings.BeRecursive <- false
    defaultProfile.ResourceDiscovererSettings.RecursiveDepth <- 3
    defaultProfile.ResourceDiscovererSettings.UseBlankExtension <- true
    [
        403; 404; 406
        500; 503; 504; 508; 509
    ]
    |> List.iter(fun c -> defaultProfile.ResourceDiscovererSettings.BlackListedStatusCodes.Add(c) |> ignore)
    defaultProfile.ResourceDiscovererSettings.BlackListedWords.Add("This error was generated by Mod_Security") |> ignore
    defaultProfile.ResourceDiscovererSettings.BlackListedWords.Add("Rate Limit Exceeded") |> ignore    
    defaultProfile.ResourceDiscovererSettings.ForbiddenDirectories.AddRange(
        [
            "manual/"; "icons/"; "icon/"
        ])
    [
        ".tmp"; ".zip"; ".bak"
    ] |> List.iter(fun e -> defaultProfile.ResourceDiscovererSettings.Extensions.Add(e) |> ignore)
    defaultProfile.ResourceDiscovererSettings.Dictionaries.Add("A8EF3FFE-7CCF-4D1F-AA0A-2248DE6A00DF") // common directories        
    defaultProfile.ResourceDiscovererSettings.Dictionaries.Add("8C2248F7-5D56-493F-B0BC-366904327B91") // Dirsearch dictionary    
    defaultProfile.ResourceDiscovererSettings.Dictionaries.Add("E4894EC1-FD53-4A24-B539-CF58C9489F89") // Struts dictionary    
    
    // fingerprinter settings
    defaultProfile.WebAppFingerprinterSettings.BeRecursive <- false
    defaultProfile.WebAppFingerprinterSettings.RaiseAnEventForEachVersionIdentified <- false
    defaultProfile.WebAppFingerprinterSettings.StopAtTheFirstApplicationIdentified <- false

    // crawler settings 
    defaultProfile.CrawlerSettings.ReCrawlPages <- false
    defaultProfile.CrawlerSettings.MaxNumOfRequestsToTheSamePage <- 100
    defaultProfile.CrawlerSettings.MaxNumberOfPagesToCrawl <- 2000
    defaultProfile.CrawlerSettings.Scope <- NavigationScope.EnteredPathAndBelow            
    defaultProfile.CrawlerSettings.ActivateAllAddOns <- true
    defaultProfile.CrawlerSettings.CrawlPageWithoutExtension <- true
    defaultProfile.CrawlerSettings.CrawlOnlyPageWithTheSpecifiedExtensions <- false
    defaultProfile.CrawlerSettings.HasLinkNavigationLimit <- false
    defaultProfile.CrawlerSettings.WebPageExtensions.AddRange(
        [
            ".flv"; ".docx"; ".gif"; ".jpeg"; ".jpg"; ".jpe"; ".png"; ".vis"; ".tif"; ".tiff"; ".psd"; ".bmp"; ".ief"; ".wbmp"; ".ras"; ".pnm"; ".pbm"; ".pgm"; ".ppm"; 
            ".rgb"; ".xbm"; ".xpm"; ".xwd"; ".djv"; ".djvu"; ".iw4"; ".iw44"; ".fif"; ".ifs"; ".dwg"; ".svf"; ".wi"; ".uff"; ".mpg"; ".mov"; ".mpeg"; ".mpeg2"; ".avi"; 
            ".asf"; ".asx"; ".wmv"; ".qt"; ".movie"; ".ice"; ".viv"; ".vivo"; ".fvi"; ".tar"; ".tgz"; ".gz"; ".zip"; ".jar"; ".cab"; ".hqx"; ".arj"; ".rar"; ".rpm"; ".ace"; 
            ".wav"; ".vox"; ".ra"; ".rm"; ".ram"; ".wma"; ".au"; ".snd"; ".mid"; ".midi"; ".kar"; ".mpga"; ".mp2"; ".mp3"; ".mp4"; ".aif"; ".aiff"; ".aifc"; ".es"; ".esl"; 
            ".pac"; ".pae"; ".a3c"; ".pdf"; ".doc"; ".xls"; ".ppt"; ".mp"; ".msi"; ".rmf"; ".smi"; ".bin"; ".m4p"; ".m4a"; ".PS"; ".EPS"; ".svg"; ".ttf"; ".ico"; ".woff"; ".woff2"
        ])
    defaultProfile.CrawlerSettings.ContentTypeToFilter.AddRange(
        [
            "image/bmp"; "image/fif"; "image/gif"; "image/ief"; "image/jpeg"; "image/png"; "image/tiff"; "image/vasa"; "image/vnd.rn-realpix"; 
            "image/x-cmu-raster"; "image/x-freehand"; "image/x-jps"; "image/x-portable-anymap"; "image/x-portable-bitmap"; "image/x-portable-graymap"; 
            "image/x-portable-pixmap"; "image/x-rgb"; "image/x-xbitmap"; "image/x-xpixmap"; "image/x-xres"; "image/x-xwindowdump"; "video/animaflex"; 
            "video/x-ms-asf"; "video/x-ms-asf-plugin"; "video/avi"; "video/msvideo"; "video/x-msvideo"; "video/avs-video"; "video/dl"; "video/x-dl"; 
            "video/x-dv"; "video/fli"; "video/x-fli"; "video/x-atomic3d-feature"; "video/gl"; "video/x-gl"; "audio/x-gsm"; "video/x-isvideo"; "audio/nspaudio"; 
            "audio/x-nspaudio"; "audio/mpeg"; "audio/x-mpequrl"; "x-music/x-midi"; "audio/midi"; "audio/x-mid"; "audio/x-midi"; "music/crescendo"; 
            "audio/x-vnd.audioexplosion.mjuicemediafile"; "video/x-motion-jpeg"; "audio/mod"; "audio/x-mod"; "audio/x-mpeg"; "video/mpeg"; "video/x-mpeq2a"; 
            "audio/mpeg3"; "audio/x-mpeg-3"; "video/x-mpeg"; "video/x-sgi-movie"; "audio/make"; "audio/vnd.qcelp"; "video/quicktime"; "video/x-qtc"; 
            "audio/x-pn-realaudio"; "audio/x-pn-realaudio-plugin"; "audio/x-realaudio"; "audio/mid"; "video/vnd.rn-realvideo"; "audio/s3m"; "video/x-scm"; 
            "audio/x-psid"; "audio/basic"; "audio/x-adpcm.tsi"; "audio/tsp-audio"; "audio/tsplayereb"; "video/vivo"; "video/vnd.vivo"; "video/vnd.vivodeo/vdo"; 
            "audio/voc"; "audio/x-voc"; "video/vosaic"; "audio/voxware"; "audio/x-twinvq-plugin"; "audio/x-twinvq"; "audio/wav"; "audio/x-wav"; 
            "video/x-amt-demorun"; "audio/xm"; "video/x-amt-showrun"
        ])
    defaultProfile.CrawlerSettings.BlacklistedPattern.AddRange(
        [
            "/logout.[a-z]+"; "/manual/"
        ]
    )

    // http requestor settings
    defaultProfile.HttpRequestorSettings.AdditionalHttpHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0")
    defaultProfile.HttpRequestorSettings.StaticExtensions.AddRange(defaultProfile.CrawlerSettings.WebPageExtensions)
    defaultProfile.HttpRequestorSettings.StaticExtensions.AddRange([".css"; ".js"])
    defaultProfile.HttpRequestorSettings.Timeout <- 3000

    // auth info
    defaultProfile.HttpRequestorSettings.Authentication.DynamicAuthParameterPatterns.AddRange(
        [
            "Token"
        ]
    )
            
    defaultProfile

let discoverHiddenResourceNotRecursive() = 
    let template = createTemplate("Identify hidden resources", "6B564326-DD6A-47F3-ADFA-A1D824032D99")
    template.Description <- "Discover resources that are not directly navigable from the web application, for example backup or test page not removed after the deployment."
    template.RunResourceDiscoverer <- true
    template.ResourceDiscovererSettings.BeRecursive <- false
    template.VulnerabilityScannerSettings.ActivateAllAddOns <- false
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Clear()

    // disable Javascript Engine
    template.HttpRequestorSettings.UseJavascriptEngineForRequest <- false
    template.CrawlerSettings.ActivateAllAddOns <- false

    template

let fingerprintWebApplication() =
    let template = createTemplate("Fingerprint web application", "9C1ED027-9187-4F3C-AE5F-1730C7B439E1")
    template.Description <- "Try to discover if a know web application is installed, for example know CMSs like Wordpress or Joomla."
    template.RunWebAppFingerprinter <- true
    template.WebAppFingerprinterSettings.BeRecursive <- false
    template.WebAppFingerprinterSettings.StopAtTheFirstApplicationIdentified <- false
    template.VulnerabilityScannerSettings.ActivateAllAddOns <- false
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Clear()

    // vulnerability scanner settings enavle only Vulnerable Web Applications AddOn
    template.RunVulnerabilityScanner <- true
    template.VulnerabilityScannerSettings.ActivateAllAddOns <- false
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(OutdatedApplication.OutdatedApplicationAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(WebApplicationVulnerability.WebApplicationVulnerabilityAddOn.Id)

    // disable Javascript Engine
    template.HttpRequestorSettings.UseJavascriptEngineForRequest <- false
    template.CrawlerSettings.ActivateAllAddOns <- false

    template
    
let fullScan() =
    let template = createTemplate("Javascript enabled full scan", "C5C97C6F-43F3-4D1C-84B3-ED0E4A83E017")
    template.Description <- "Run a full scan with all the checks enabled and the Javascript engine (this feature is still experimental). This is an invasive scan and may cause availability problems to the scanned application."

    template.RunCrawler <- true
    template.RunResourceDiscoverer <- true
    template.RunWebAppFingerprinter <- true
    template.RunVulnerabilityScanner <- true    
    template

let fullScanNoJavascript() =
    let template = createTemplate("Full scan no Javascript", "8D870861-978E-4852-BEE0-8DF218939452")
    template.Description <- "Run a full scan with all the checks enabled but without the parsing of Javascript. This will speed up the scan process. If the tested web site doesn't create any dynamic link/form via Javascript, this profile is the suggested one for a full scan."

    template.RunCrawler <- true
    template.RunResourceDiscoverer <- true
    template.RunWebAppFingerprinter <- true
    template.RunVulnerabilityScanner <- true    

    // disable Javascript Engine
    template.HttpRequestorSettings.UseJavascriptEngineForRequest <- false
    
    // disable the Crawler parser
    template.CrawlerSettings.ActivateAllAddOns <- false    
    template.CrawlerSettings.AddOnIdsToActivate.Clear()
    template.CrawlerSettings.AddOnIdsToActivate.AddRange
        ([
            FormLinkScraper.AddOnId
            HeaderRedirectLinkScraper.AddOnId
            HyperLinkScraper.AddOnId
            MetadataLinkScraper.AddOnId
        ])

    template

let notInvasive() =
    let template = createTemplate("Run a not invasive scan", "FA327046-7FFF-4934-8B45-9323FE47D209")        
    template.Description <- "Run a scan with settings that are sure to be safe for the availability of the server"

    // disable Javascript Engine
    template.HttpRequestorSettings.UseJavascriptEngineForRequest <- false

    // crawler settings
    template.RunCrawler <- true
    template.CrawlerSettings.ActivateAllAddOns <- true
    template.CrawlerSettings.ReCrawlPages <- false
    template.CrawlerSettings.SubmitPost <- false
    
    // disable the Crawler parser
    template.CrawlerSettings.ActivateAllAddOns <- false
    template.CrawlerSettings.AddOnIdsToActivate.Clear()
    template.CrawlerSettings.AddOnIdsToActivate.AddRange
        ([
            FormLinkScraper.AddOnId
            HeaderRedirectLinkScraper.AddOnId
            HyperLinkScraper.AddOnId
            MetadataLinkScraper.AddOnId
        ])

    // web app fingerprinter settings
    template.RunWebAppFingerprinter <- true
    template.WebAppFingerprinterSettings.BeRecursive <- true
    template.WebAppFingerprinterSettings.StopAtTheFirstApplicationIdentified <- false

    // discoverer settings
    template.RunResourceDiscoverer <- true
    template.ResourceDiscovererSettings.BeRecursive <- false
    template.ResourceDiscovererSettings.Dictionaries.Clear()
    template.ResourceDiscovererSettings.Dictionaries.Add("8C2248F7-5D56-493F-B0BC-366904327B91") // Dirbuster dictionary
        
    // vulnerability scanner settings
    template.RunVulnerabilityScanner <- true
    template.VulnerabilityScannerSettings.ActivateAllAddOns <- false
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(DirectoryListing.DirectoryListingAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(OutdatedApplication.OutdatedApplicationAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(InformationLeakage.InformationLeakageAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(WebApplicationVulnerability.WebApplicationVulnerabilityAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(SecurityHeaders.XContentTypeOptionsAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(SecurityHeaders.XXSSProtectionAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(SecurityHeaders.XFrameOptionsAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(SecurityHeaders.PublicKeyPinsAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(SecurityHeaders.ContentSecurityPolicyAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(SecurityHeaders.StrictTransportSecurityAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(ErrorMessages.ErrorMessagesAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(VCSInformationDisclosure.VCSInformationDisclosureAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(PhpInfoInformationDisclosure.PhpInfoInformationDisclosureAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(PasswordFieldCheck.PasswordSentOverHttpAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(PasswordFieldCheck.MissingAutocompleteOffAttributeAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(CookieFlags.MissingHttpOnlyCookieFlagAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(CookieFlags.MissingSecureCookieFlagAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(ExposedSessionVariables.ExposedSessionVariablesAddOn.Id)
    template.VulnerabilityScannerSettings.AddOnIdsToActivate.Add(SSLTest.SSLTestAddOn.Id)
        
    template

let templates = [
    discoverHiddenResourceNotRecursive()
    fingerprintWebApplication()    
    notInvasive()
    fullScan()
    fullScanNoJavascript()
]

let dumpTemplate(template: TemplateProfile) =
    let filename = template.Name + ".xml"
    File.WriteAllText(filename, template.ToXml())
    Console.WriteLine("Written template to file: " + filename)

let getTemplateContents() =
    templates
    |> List.map(fun t -> (t.Name, t.ToXml()))


// dump all templates if passed the -d or --dump switch
if Environment.GetCommandLineArgs() |> Array.exists(fun arg -> arg = "--dump" || arg = "-d") then
    templates
    |> List.iter dumpTemplate