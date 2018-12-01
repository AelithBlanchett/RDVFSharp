namespace RDVFSharp
{
    class Program
    {
        public static RendezvousFighting RDV { get; set; }
        public static bool IsDebugging { get; set; } = false;

        static void Main()
        {
            RDV = new RendezvousFighting(new DataContext.RDVFDataContext(), "ADH-7c167eb564e62f82b40f", IsDebugging);
            RDV.Run();
        }
    }
}
