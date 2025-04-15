import { Routes } from '@angular/router';

export const routes: Routes = [
  {path:"" , redirectTo:"home" , pathMatch:'full'  },
  {path:"home", loadComponent:() => import('./features/home/home.component').then(m => m.HomeComponent), title:"home"},
  // {path:"login", loadComponent:() => import('./features/login/login.component').then(m => m.LoginComponent), title:"login"},
  // {path:"register", loadComponent:() => import('./features/register/register.component').then(m => m.RegisterComponent), title:"register"},
  {path:"listing-details", loadComponent:() => import('./features/listing-details/listing-details.component').then(m => m.ListingDetailsComponent), title:"Listing-Details"},
  {
    path:"hosting",
    loadComponent:() => import('./features/airbnb-your-home/airbnb-your-home.component').then(m => m.AirbnbYourHomeComponent),
    title:"hosting",
    children:[
      {path:"today", loadComponent:() => import('./features/host-today/host-today.component').then(m => m.HostTodayComponent), title:"today"},
      {path:"listings", loadComponent:() => import('./features/host-listing/host-listing.component').then(m => m.HostListingComponent), title:"listings"},
      {path:"", redirectTo:"today", pathMatch:'full'  },
    ]
  },
  {path:"dashboard", loadComponent:() => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent), title:"dashboard"},
  {path:"**", redirectTo:"home" , pathMatch:'full'  } // Wildcard route for a 404 page
];
