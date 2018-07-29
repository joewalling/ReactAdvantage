import React from 'react';
import Enzyme, { shallow, mount } from 'enzyme';
import simulant from 'simulant';

import HideOnClickOutsideContainer from './index';

it('renders correctly', () => {
    const component = shallow(<HideOnClickOutsideContainer />);

    expect(component).toMatchSnapshot();
});


test('container is disappearing after click on outside', () => {
    const onHide = jest.fn();

    const component = mount(
        <HideOnClickOutsideContainer onHide={onHide}>
            <div>Content</div>
        </HideOnClickOutsideContainer>
    );

    simulant.fire(document, 'mousedown');

    expect(onHide).toBeCalled();
});
