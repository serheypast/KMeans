import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FirstStepLab1Component } from './first-step-lab1.component';

describe('FirstStepLab1Component', () => {
  let component: FirstStepLab1Component;
  let fixture: ComponentFixture<FirstStepLab1Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FirstStepLab1Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FirstStepLab1Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
