import roles from './roles';

export class RolesService {
  getRoles() {
    return new Promise(resolve => {
      resolve(roles);
    });
  }

  getRole(id) {
    return new Promise(resolve => {
      resolve(roles.find(d => d.id === id));
    });
  }
}

export default RolesService;
