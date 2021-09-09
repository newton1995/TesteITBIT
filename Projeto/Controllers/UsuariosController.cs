using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TesteITBIT.Models;

namespace TesteITBIT.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Contexto _context;

        public UsuariosController(Contexto context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var listaAtivo = new[]
            {
                new SelectListItem { Value = "A", Text = "Ativo" },
                new SelectListItem { Value = "I", Text = "Inativo" },
            };
            ViewBag.FiltroAtivo = new SelectList(listaAtivo, "Value", "Text");

            var contexto = _context.Usuarios.Where(x => x.Ativo == true).Include(u => u.Sexo).OrderBy(x => x.Nome);

            return View(await contexto.ToListAsync());
        }


        public async Task<IActionResult> Filtro(string filtroNome, string filtroAtivo)
        {
            if (filtroNome == null)
            {
                filtroNome = "";
            }

            bool ativo;

            if (filtroAtivo == "A")
            {
                ativo = true;

            }
            else
            {
                ativo = false;
            }

            var contexto = _context.Usuarios
                .Where(x => x.Nome.Contains(filtroNome) && x.Ativo == ativo)
                .Include(u => u.Sexo)
                .OrderBy(x => x.Nome);


            var listaAtivo = new[]
             {
                new SelectListItem { Value = "A", Text = "Ativo" ,Selected = ativo},
                new SelectListItem { Value = "I", Text = "Inativo" ,Selected = ativo},
            };
            ViewBag.FiltroAtivo = new SelectList(listaAtivo, "Value", "Text");

            ViewBag.FiltroNome = filtroNome;

            return View("Index", await contexto.ToListAsync());
        }



        public IActionResult Create()
        {
            ViewData["SexoId"] = new SelectList(_context.Sexos, "SexoId", "Descricao");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,Nome,DataNascimento,Email,Senha,Ativo,SexoId")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Ativo = true;
                _context.Add(usuario);
                TempData["Success"] = "Cadastrado com sucesso";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SexoId"] = new SelectList(_context.Sexos, "SexoId", "Descricao", usuario.SexoId);
            return View(usuario);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["SexoId"] = new SelectList(_context.Sexos, "SexoId", "Descricao", usuario.SexoId);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,Nome,DataNascimento,Email,Senha,Ativo,SexoId")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Alterado com sucesso";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SexoId"] = new SelectList(_context.Sexos, "SexoId", "Descricao", usuario.SexoId);
            return View(usuario);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Registro excluido com sucesso";
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}
