using Empresa.Proyecto.Core.Entities;
using Empresa.Proyecto.Core.Interfaces;
using Empresa.Proyecto.Infra.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Empresa.Proyecto.Web.Pages.Catalogos
{
    public class NewEntityFormModel : PageModel
    {
        private readonly IAsyncRepository<NewEntity> _newEntityRepo;
        private readonly IAsyncRepository<SimpleEntity> _simpleEntityRepo;
        private readonly MyProjectContext _context;

        public NewEntityFormModel( IAsyncRepository<NewEntity> newEntityRepo, IAsyncRepository<SimpleEntity> simpleEntityRepo, MyProjectContext context)
        {
            _newEntityRepo = newEntityRepo;
            _simpleEntityRepo = simpleEntityRepo;
            _context = context;
        }

        [BindProperty]
        public string Name { get; set; } = null!; 

        [BindProperty]
        public int SelectedOptionId { get; set; } 

        public List<SimpleEntity> Options { get; set; } = new(); // Lista para cargar el select.

        public async Task OnGetAsync()
        {
            // Carga opciones al select desde SimpleEntity.
            Options = (await _simpleEntityRepo.ListAllAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Obtiene la opción seleccionada.
            var selectedOption = await _simpleEntityRepo.GetbyIdAsync(SelectedOptionId);

            // Crea un nuevo registro de NewEntity.
            var newEntity = new NewEntity
            {
                Name = Name,
                Option = selectedOption,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };

            await _newEntityRepo.AddAsync(newEntity);

            return RedirectToPage("/Catalogos/NewEntityForm");
        }

        public async Task<JsonResult> OnPostCatalog()
        {
            var catalog = await _context.NewEntity
                .Include(e => e.Option)
                .ToListAsync();

            return new JsonResult(new { data = catalog });
        }
    }
}
