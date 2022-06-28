
# CaptchaNoJS
CaptchaNoJS protects ASP.NET/Core forms from spam, it's a self-hosted .NET library: no 3rd-party servers are involved in the generation, displaying, and validation of Captcha challenges.

###### Captcha.CaptchaType.Line
![download](https://user-images.githubusercontent.com/6315083/176263707-49d10af1-4a97-4b97-a538-093cbee75188.png)
###### Captcha.CaptchaType.Circle
![download](https://user-images.githubusercontent.com/6315083/176263165-2ec4e4ee-11b4-4471-be68-81c72a4e04dd.png)

## Usage example

##### Controller :
```csharp
// GET: Example/Create
public IActionResult Create()
{
    Captcha captcha = new Captcha(200, 80, 6);
    ViewData["b64"] = captcha.GenerateAsB64(); // or captcha.GenerateAsB64(Captcha.CaptchaType.Circle);
    HttpContext.Session.SetString("captchaAnswer", captcha.GetAnswer());
    return View();
}
```

```csharp
// POST: Example/Create
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(String captcha)
{
    if (captcha == HttpContext.Session.GetString("captchaAnswer")) {
  ....
    }
}
```
##### View :
```html
<form asp-action="Create">
    <div class="form-group">
        <label for="Captcha"> Captcha </label>
        <img src='data:image/Png;base64,@ViewData["b64"]' />
        <input type="text" class="form-control" name="captcha" />
   </div>
   <div class="form-group">
        <input type="submit" value="Save" class="btn btn-primary" />
   </div>
</form>
```
