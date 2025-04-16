import { Routes } from '@angular/router';

export const routes: Routes = [
  {path:"" , redirectTo:"home" , pathMatch:'full'  },
  {path:"home", loadComponent:() => import('./features/home/home.component').then(m => m.HomeComponent), title:"home"},
  // {path:"login", loadComponent:() => import('./features/login/login.component').then(m => m.LoginComponent), title:"login"},
  // {path:"register", loadComponent:() => import('./features/register/register.component').then(m => m.RegisterComponent), title:"register"},
  {path:"listing-details", loadComponent:() => import('./features/listing-details/listing-details.component').then(m => m.ListingDetailsComponent), title:"Listing-Details"},
  {
    path:"hosting",
    loadComponent:() => import('./features/AirbnbYourHome/airbnb-your-home/airbnb-your-home.component').then(m => m.AirbnbYourHomeComponent),
    title:"hosting",
    children:[
      {path:"today", loadComponent:() => import('./features/AirbnbYourHome/host-today/host-today.component').then(m => m.HostTodayComponent), title:"today"},
      {path:"listings", loadComponent:() => import('./features/AirbnbYourHome/host-listing/host-listing.component').then(m => m.HostListingComponent), title:"listings"},
      {path:"become-a-host", loadComponent:() => import('./features/AirbnbYourHome/become-a-host/become-a-host.component').then(m => m.BecomeAHostComponent), title:"become-a-host",
        children:[
          {path:"", loadComponent:() => import('./features/AirbnbYourHome/host-drafts/host-drafts.component').then(m => m.HostDraftsComponent), title:"host-drafts"},
          {path:"add-listing", loadComponent:() => import('./features/AirbnbYourHome/add-listing/add-listing.component').then(m => m.AddListingComponent), title:"add-listing"},
        ]
      },
      {path:"", redirectTo:"today", pathMatch:'full'  },
    ]
  },
  {path:"dashboard", loadComponent:() => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent), title:"dashboard"},
  {path:"**", redirectTo:"home" , pathMatch:'full'  } // Wildcard route for a 404 page
];
