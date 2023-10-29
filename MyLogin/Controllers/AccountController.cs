using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyLogin.Domain.IdentityEntities;
using MyLogin.Enums;
using MyLogin.ModelsForViews;

namespace MyLogin.Controllers
{
    //[AllowAnonymous]

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }


        [HttpGet]
        [Authorize("NotAuthorized")]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> Register(RegisterDTO obj)
        {
            if(ModelState.IsValid == false)
            {
                return View(obj);
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = obj.Email,
                PersonName = obj.PersonName,
                PhoneNumber = obj.Phone,
                UserName = obj.Email,
            };

            IdentityResult result =  await _userManager.CreateAsync(user,obj.ConfirmPassword);
            
            if (result.Succeeded)
            {
                if (obj.UserType== MyLogin.Enums.UserTypesOptions.Admin)
                {
                    if (await _roleManager.FindByNameAsync(UserTypesOptions.Admin.ToString())is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole()
                        {
                            Name = UserTypesOptions.Admin.ToString(),
                        };
                       await _roleManager.CreateAsync(applicationRole);
                    }
                   await _userManager.AddToRoleAsync(user,UserTypesOptions.Admin.ToString());
                }
                else
                {
                    if (await _roleManager.FindByNameAsync(UserTypesOptions.User.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole()
                        {
                            Name = UserTypesOptions.User.ToString(),
                        };
                        await _roleManager.CreateAsync(applicationRole);
                    }
                    await _userManager.AddToRoleAsync(user, UserTypesOptions.User.ToString());
                }

                await _signInManager.SignInAsync(user, isPersistent: false);//login cookie is available true/false

               return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
                return View(obj);
            }
            //return RedirectToAction(nameof(HomeController.Index),"Home");
        }


        [HttpGet]
        [Authorize("NotAuthorized")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> Login(LoginDTO  obj, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var result = await _signInManager.PasswordSignInAsync(obj.Email, obj.Password, isPersistent: false,lockoutOnFailure:false);

            if (result.Succeeded)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(obj.Email);
                if (user!=null)
                {
                    if (await _userManager.IsInRoleAsync(user,UserTypesOptions.Admin.ToString()))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                }
                // Sign in the user and create the Identity cookie
                await _signInManager.SignInAsync(user, isPersistent: false);

                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }
                return RedirectToAction("Index", "Test");
            }
            ModelState.AddModelError("Login", "Invalid username or password");
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public async Task<IActionResult> IsEmailAlreadyRegisterd(string email)
        {
            ApplicationUser user = await  _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
    }
}
