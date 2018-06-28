export const typeDefs = `
type User {
   id: ID!                # "!" denotes a required field
   roleName: String
}
# This type specifies the entry points into our API. In this case
# there is only one - "channels" - which returns a list of channels.
type Query {
   roles: [User]    # "[]" means this is a list of channels
}
`;