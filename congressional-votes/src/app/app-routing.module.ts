import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SenatorDetailComponent } from './senator-detail/senator-detail.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  { path: '', component: HomeComponent, data: { animation: 'Home' } },
  {
    path: 'senator-details',
    component: SenatorDetailComponent,
    data: { animation: 'SenatorDetails' },
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
