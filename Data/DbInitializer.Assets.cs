using System.Text;

namespace RealEstatePortfolio.Data
{
    public static partial class DbInitializer
    {
        private static void GeneratePlaceholderAssets(string webRootPath)
        {
            var imagesDir = Path.Combine(webRootPath, "images", "properties");
            var brochuresDir = Path.Combine(webRootPath, "brochures");
            Directory.CreateDirectory(imagesDir);
            Directory.CreateDirectory(brochuresDir);

            var assetList = new Dictionary<string, (string title, string category, string location, string bg1, string bg2, string accent, int type)>
            {
                // Lumina
                { "lumina-1.svg", ("The Lumina Horizon Tower", "EXTERIOR ELEVATION", "Chelsea • New York", "#090d16", "#1e293b", "#38bdf8", 1) },
                { "lumina-2.svg", ("Grand Living Salon & Terrace", "PENTHOUSE INTERIOR", "Chelsea • New York", "#111827", "#1f2937", "#f59e0b", 2) },
                { "lumina-3.svg", ("Travertine Heated Lap Pool", "WELLNESS & SPA", "Chelsea • New York", "#082f49", "#0f172a", "#06b6d4", 3) },
                { "lumina-plan.svg", ("Duplex Sky Villa 4,850 Sq Ft", "ARCHITECTURAL BLUEPRINT", "Chelsea • New York", "#0f172a", "#1e293b", "#38bdf8", 4) },

                // Marina
                { "marina-1.svg", ("Marina Azure Residences", "WATERFRONT ELEVATION", "Dubai Marina • UAE", "#022c22", "#0f172a", "#10b981", 1) },
                { "marina-2.svg", ("Cantilevered Horizon Pool", "OUTDOOR AMENITY", "Dubai Marina • UAE", "#083344", "#0f172a", "#06b6d4", 3) },
                { "marina-3.svg", ("Master Oceanfront Suite", "RESIDENCE INTERIOR", "Dubai Marina • UAE", "#1c1917", "#292524", "#eab308", 2) },

                // Solaris
                { "solaris-1.svg", ("Solaris Eco-Sanctuary", "BIOPHILIC ARCHITECTURE", "Bukit Timah • Singapore", "#064e3b", "#022c22", "#34d399", 1) },
                { "solaris-2.svg", ("Mass Timber Curved Atrium", "INTERIOR LOBBY", "Bukit Timah • Singapore", "#14532d", "#1c1917", "#84cc16", 2) },
                { "solaris-3.svg", ("Rooftop Solar Eco-Garden", "RENEWABLE AMENITY", "Bukit Timah • Singapore", "#042f2e", "#0f172a", "#10b981", 3) },

                // Apex
                { "apex-1.svg", ("Apex Pinnacle Center", "COMMERCIAL LANDMARK", "City of London • UK", "#18181b", "#09090b", "#6366f1", 1) },
                { "apex-2.svg", ("Column-Free Trading Floor", "COMMERCIAL INTERIOR", "City of London • UK", "#1e1e24", "#111827", "#818cf8", 2) },
                { "apex-3.svg", ("Executive Sky Terrace", "ROOFTOP OBSERVATORY", "City of London • UK", "#0f172a", "#18181b", "#a855f7", 3) },

                // Aurora
                { "aurora-1.svg", ("Aurora Glasshouse Villas", "MODERN JAPANESE", "Roppongi • Tokyo", "#2e1065", "#0f172a", "#c084fc", 1) },
                { "aurora-2.svg", ("Thermal Onsen Sky Terrace", "PRIVATE WELLNESS", "Roppongi • Tokyo", "#3b0764", "#18181b", "#e879f9", 3) },
                { "aurora-3.svg", ("Hinoki Wood Zen Lounge", "RESIDENCE INTERIOR", "Roppongi • Tokyo", "#1c1917", "#27272a", "#fbbf24", 2) },

                // Vanguard
                { "vanguard-1.svg", ("Vanguard Harbor Crest", "WATERFRONT TOWER", "Barangaroo • Sydney", "#082f49", "#0f172a", "#38bdf8", 1) },
                { "vanguard-2.svg", ("Sydney Opera House Vista", "PANORAMIC TERRACE", "Barangaroo • Sydney", "#0c4a6e", "#1e293b", "#0ea5e9", 2) },
                { "vanguard-3.svg", ("Harborfront Yacht Club", "PRIVATE AMENITY", "Barangaroo • Sydney", "#172554", "#090d16", "#60a5fa", 3) }
            };

            foreach (var kvp in assetList)
            {
                var filePath = Path.Combine(imagesDir, kvp.Key);
                var svgContent = GenerateCuratedSvg(
                    kvp.Value.title, 
                    kvp.Value.category, 
                    kvp.Value.location, 
                    kvp.Value.bg1, 
                    kvp.Value.bg2, 
                    kvp.Value.accent, 
                    kvp.Value.type);
                File.WriteAllText(filePath, svgContent, Encoding.UTF8);
            }

            var brochureNames = new[]
            {
                "the-lumina-horizon-tower-brochure.pdf",
                "marina-azure-brochure.pdf",
                "solaris-eco-sanctuary-brochure.pdf",
                "apex-pinnacle-brochure.pdf",
                "aurora-glasshouse-brochure.pdf",
                "vanguard-harbor-crest-brochure.pdf"
            };

            foreach (var bName in brochureNames)
            {
                var bPath = Path.Combine(brochuresDir, bName);
                if (!File.Exists(bPath))
                {
                    string pdfText = "%PDF-1.4\n1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>\nendobj\n4 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold >>\nendobj\n5 0 obj\n<< /Length 220 >>\nstream\nBT\n/F1 22 Tf\n50 720 Td\n(ARCHITECTURAL SPECIFICATION & FLOORPLANS) Tj\n/F1 12 Tf\n0 -35 Td\n(Bespoke Property Portfolio & Environmental Performance Audit) Tj\nET\nendstream\nendobj\nxref\n0 6\n0000000000 65535 f \n0000000009 00000 n \n0000000058 00000 n \n0000000115 00000 n \n0000000228 00000 n \n0000000305 00000 n \ntrailer\n<< /Size 6 /Root 1 0 R >>\nstartxref\n580\n%%EOF";
                    File.WriteAllBytes(bPath, Encoding.ASCII.GetBytes(pdfText));
                }
            }
        }

        private static string GenerateCuratedSvg(string title, string category, string location, string bg1, string bg2, string accent, int type)
        {
            // type 1 = Tower Elevation, 2 = Interior Architecture, 3 = Pool/Amenity, 4 = Floorplan CAD
            string illustration = type switch
            {
                1 => $@"
    <!-- Skyscraper Elevation -->
    <g transform=""translate(600, 390)"">
      <!-- Glow -->
      <circle cx=""0"" cy=""-50"" r=""220"" fill=""{accent}"" opacity=""0.15"" filter=""blur(40px)"" />
      <!-- Facade silhouette -->
      <path d=""M -120 220 L -120 -180 L 0 -260 L 120 -180 L 120 220 Z"" fill=""rgba(255,255,255,0.04)"" stroke=""rgba(255,255,255,0.2)"" stroke-width=""2"" />
      <path d=""M 0 -260 L 0 220"" stroke=""rgba(255,255,255,0.15)"" stroke-width=""1.5"" />
      <!-- Floor lines & warm windows -->
      <line x1=""-120"" y1=""-120"" x2=""120"" y2=""-120"" stroke=""rgba(255,255,255,0.1)"" />
      <line x1=""-120"" y1=""-60"" x2=""120"" y2=""-60"" stroke=""rgba(255,255,255,0.1)"" />
      <line x1=""-120"" y1=""0"" x2=""120"" y2=""0"" stroke=""rgba(255,255,255,0.1)"" />
      <line x1=""-120"" y1=""60"" x2=""120"" y2=""60"" stroke=""rgba(255,255,255,0.1)"" />
      <line x1=""-120"" y1=""120"" x2=""120"" y2=""120"" stroke=""rgba(255,255,255,0.1)"" />
      <line x1=""-120"" y1=""180"" x2=""120"" y2=""180"" stroke=""rgba(255,255,255,0.1)"" />
      <!-- Spire -->
      <line x1=""0"" y1=""-260"" x2=""0"" y2=""-330"" stroke=""{accent}"" stroke-width=""3"" />
      <circle cx=""0"" cy=""-330"" r=""5"" fill=""{accent}"" />
      <!-- Architectural framing lines -->
      <path d=""M -240 220 L -120 160"" stroke=""rgba(255,255,255,0.15)"" stroke-dasharray=""4 4"" />
      <path d=""M 240 220 L 120 160"" stroke=""rgba(255,255,255,0.15)"" stroke-dasharray=""4 4"" />
    </g>",
                2 => $@"
    <!-- Interior Architecture -->
    <g transform=""translate(600, 390)"">
      <rect x=""-300"" y=""-180"" width=""600"" height=""340"" rx=""12"" fill=""rgba(255,255,255,0.03)"" stroke=""rgba(255,255,255,0.15)"" stroke-width=""2"" />
      <!-- Perspective lines -->
      <line x1=""-300"" y1=""-180"" x2=""-120"" y2=""-60"" stroke=""rgba(255,255,255,0.2)"" />
      <line x1=""300"" y1=""-180"" x2=""120"" y2=""-60"" stroke=""rgba(255,255,255,0.2)"" />
      <line x1=""-300"" y1=""160"" x2=""-120"" y2=""90"" stroke=""rgba(255,255,255,0.2)"" />
      <line x1=""300"" y1=""160"" x2=""120"" y2=""90"" stroke=""rgba(255,255,255,0.2)"" />
      <!-- Floor-to-ceiling glass frame -->
      <rect x=""-120"" y=""-60"" width=""240"" height=""150"" fill=""rgba(56, 189, 248, 0.08)"" stroke=""{accent}"" stroke-width=""1.5"" />
      <!-- Skyline silhouette through glass -->
      <polygon points=""-90,90 -90,0 -50,0 -50,90"" fill=""rgba(255,255,255,0.1)"" />
      <polygon points=""-40,90 -40,-30 10,-30 10,90"" fill=""rgba(255,255,255,0.15)"" />
      <polygon points=""20,90 20,-10 70,-10 70,90"" fill=""rgba(255,255,255,0.08)"" />
      <!-- Minimalist Lounge Sofa -->
      <rect x=""-90"" y=""100"" width=""180"" height=""35"" rx=""8"" fill=""rgba(255,255,255,0.12)"" stroke=""rgba(255,255,255,0.25)"" />
    </g>",
                3 => $@"
    <!-- Infinity Pool / Spa -->
    <g transform=""translate(600, 390)"">
      <ellipse cx=""0"" cy=""30"" rx=""260"" ry=""110"" fill=""rgba(6, 182, 212, 0.12)"" stroke=""{accent}"" stroke-width=""2"" />
      <!-- Water ripple waves -->
      <path d=""M -180 30 Q -90 15 0 30 T 180 30"" fill=""none"" stroke=""rgba(255,255,255,0.3)"" stroke-width=""1.5"" />
      <path d=""M -140 55 Q -70 40 0 55 T 140 55"" fill=""none"" stroke=""rgba(255,255,255,0.2)"" stroke-width=""1"" />
      <path d=""M -100 80 Q -50 70 0 80 T 100 80"" fill=""none"" stroke=""rgba(255,255,255,0.15)"" stroke-width=""1"" />
      <!-- Pergola Columns -->
      <line x1=""-200"" y1=""-140"" x2=""-200"" y2=""20"" stroke=""rgba(255,255,255,0.25)"" stroke-width=""3"" />
      <line x1=""200"" y1=""-140"" x2=""200"" y2=""20"" stroke=""rgba(255,255,255,0.25)"" stroke-width=""3"" />
      <line x1=""-230"" y1=""-140"" x2=""230"" y2=""-140"" stroke=""rgba(255,255,255,0.35)"" stroke-width=""4"" />
    </g>",
                _ => $@"
    <!-- CAD Blueprint Floorplan -->
    <g transform=""translate(600, 390)"" stroke=""rgba(255,255,255,0.25)"" stroke-width=""2"" fill=""none"">
      <rect x=""-240"" y=""-140"" width=""480"" height=""280"" rx=""4"" fill=""rgba(255,255,255,0.03)"" />
      <!-- Rooms Partition -->
      <line x1=""-240"" y1=""0"" x2=""60"" y2=""0"" />
      <line x1=""60"" y1=""-140"" x2=""60"" y2=""140"" />
      <line x1=""-90"" y1=""0"" x2=""-90"" y2=""140"" />
      <!-- Terrace Curve -->
      <path d=""M 60 -140 Q 240 -140 240 0 T 60 140"" fill=""rgba(56, 189, 248, 0.05)"" stroke=""{accent}"" />
      <!-- Room Labels -->
      <text x=""-160"" y=""-60"" fill=""rgba(255,255,255,0.4)"" font-family=""sans-serif"" font-size=""11"" stroke=""none"">MASTER SUITE</text>
      <text x=""-160"" y=""70"" fill=""rgba(255,255,255,0.4)"" font-family=""sans-serif"" font-size=""11"" stroke=""none"">LIVING SALON</text>
      <text x=""120"" y=""5"" fill=""{accent}"" font-family=""sans-serif"" font-size=""11"" stroke=""none"">SKY TERRACE</text>
    </g>"
            };

            return $@"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 1200 800"" width=""100%"" height=""100%"">
  <defs>
    <linearGradient id=""bg"" x1=""0%"" y1=""0%"" x2=""100%"" y2=""100%"">
      <stop offset=""0%"" stop-color=""{bg1}"" />
      <stop offset=""100%"" stop-color=""{bg2}"" />
    </linearGradient>
    <pattern id=""dots"" width=""30"" height=""30"" patternUnits=""userSpaceOnUse"">
      <circle cx=""2"" cy=""2"" r=""1"" fill=""rgba(255,255,255,0.06)"" />
    </pattern>
  </defs>

  <rect width=""1200"" height=""800"" fill=""url(#bg)""/>
  <rect width=""1200"" height=""800"" fill=""url(#dots)""/>

  <!-- Architectural Background Grid Accent -->
  <circle cx=""600"" cy=""390"" r=""290"" fill=""none"" stroke=""rgba(255,255,255,0.05)"" stroke-width=""1"" />
  <circle cx=""600"" cy=""390"" r=""380"" fill=""none"" stroke=""rgba(255,255,255,0.03)"" stroke-dasharray=""6 6"" />

  {illustration}

  <!-- Header Badge -->
  <g transform=""translate(60, 60)"">
    <rect x=""0"" y=""0"" width=""190"" height=""32"" rx=""16"" fill=""rgba(255,255,255,0.08)"" stroke=""rgba(255,255,255,0.15)"" stroke-width=""1""/>
    <text x=""95"" y=""20"" font-family=""'Plus Jakarta Sans', system-ui, -apple-system, sans-serif"" font-size=""11"" font-weight=""700"" fill=""{accent}"" text-anchor=""middle"" letter-spacing=""1.5"">{category}</text>
  </g>

  <!-- Editorial Info Card at Bottom -->
  <g transform=""translate(60, 640)"">
    <rect x=""0"" y=""0"" width=""1080"" height=""100"" rx=""16"" fill=""rgba(15, 23, 42, 0.85)"" stroke=""rgba(255, 255, 255, 0.12)"" stroke-width=""1""/>
    <text x=""40"" y=""44"" font-family=""'Playfair Display', Georgia, serif"" font-size=""26"" font-weight=""700"" fill=""#ffffff"">{title}</text>
    <text x=""40"" y=""76"" font-family=""'Plus Jakarta Sans', system-ui, sans-serif"" font-size=""14"" font-weight=""400"" fill=""#94a3b8"">{location}</text>
    <text x=""1040"" y=""58"" font-family=""'Plus Jakarta Sans', system-ui, sans-serif"" font-size=""13"" font-weight=""700"" fill=""{accent}"" text-anchor=""end"" letter-spacing=""2"">RECENT GROUP</text>
  </g>
</svg>";
        }
    }
}
