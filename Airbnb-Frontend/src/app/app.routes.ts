import { Routes } from '@angular/router';

export const routes: Routes = [
  {path:"" , redirectTo:"home" , pathMatch:'full'  },
  {path:"home", loadComponent:() => import('./features/home/home.component').then(m => m.HomeComponent), title:"home"},
  {path:"login", loadComponent:() => import('./features/login/login.component').then(m => m.LoginComponent), title:"login"},
  {path:"register", loadComponent:() => import('./features/register/register.component').then(m => m.RegisterComponent), title:"register"},
  {path:"**", redirectTo:"home" , pathMatch:'full'  } // Wildcard route for a 404 page
];
