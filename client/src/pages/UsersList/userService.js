import { ApolloClient } from 'apollo-client';
import gql from 'graphql-tag';
import { HttpLink } from 'apollo-link-http';
import { InMemoryCache } from 'apollo-cache-inmemory';
import { makeExecutableSchema, addMockFunctionsToSchema } from 'graphql-tools';
import { mockNetworkInterfaceWithSchema } from 'apollo-test-utils';

import { AuthService } from '../../services/AuthService';
import { typeDefs } from './schema';
const schema = makeExecutableSchema({ typeDefs });
addMockFunctionsToSchema({ schema });
const mockNetworkInterface = mockNetworkInterfaceWithSchema({ schema });

let authService = new AuthService();

export class UsersService {
  async getUsers() {
    let authUser = await authService.ensureAuthorized();

    const link = new HttpLink({
      uri: `${process.env.REACT_APP_API_URI}/graphql`,
      headers: {
        'content-type': 'application/json',
        Authorization: `Bearer ${authUser.access_token}`,
      },
    });

    const client = new ApolloClient({
      link: link,
      networkInterface: mockNetworkInterface,
      cache: new InMemoryCache(),
    });

    const res = await client.query({
      query: gql`
        query FeedQuery {
          users {
            userName
            firstName
            lastName
            email
            isActive
          }
        }
      `,
    });

    return res.data.users;
  }
}
